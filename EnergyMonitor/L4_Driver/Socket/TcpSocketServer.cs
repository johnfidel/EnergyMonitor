using System.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using EnergyMonitor.Utils;
using System.Threading.Tasks;

namespace EnergyMonitor.L4_Driver.Socket {
  class TcpSocketServer : TaskBase, IDevice<string> {

    public List<TcpClient> Clients { get; private set; }

    private TcpListener Server { get; set; }

    private Task AcceptClientsTask { get; set; }

    private object _syncObject;

    public TcpSocketServer(Int32 listeningPort) : base(100, true) {
      try {
        IPAddress localAddr = IPAddress.Parse("0.0.0.0");
        Clients = new List<TcpClient>();
        Server = new TcpListener(localAddr, listeningPort);
        _syncObject = new object();

        // Start listening for client requests.
        Server.Start();

        Logging.Instance().Log(new LogMessage("TcpServer started"));
      }
      catch (Exception e) {
        Logging.Instance().Log(new LogMessage($"Unexcepted Error {e.Message}"));
      }

    }

    public event DataReceivedEvent<string> DataReceivedEvent;

    protected override void Run() {
      try {
        List<TcpClient> clientsToRemove = new List<TcpClient>();

        if (AcceptClientsTask == null) {
          AcceptClientsTask = Task.Factory.StartNew(() => {
            while (!CancellationToken.IsCancellationRequested) {
              Logging.Instance().Log(new LogMessage($"Wait for Tcp connection..."));
              TcpClient client = Server.AcceptTcpClient();
              lock (_syncObject) {
                Clients.Add(client);
              }
              Logging.Instance().Log(new LogMessage($"Accepted Tcp connection ({client.Client.RemoteEndPoint})"));
            }
          }, CancellationToken.Token);
        }

        lock (_syncObject) {
          foreach (var client in Clients) {
            if (client.GetState() == System.Net.NetworkInformation.TcpState.Established) {
              var stream = client.GetStream();
              if (stream.DataAvailable) {
                byte[] buffer = new byte[4096];
                var size = stream.Read(buffer);
                Array.Resize(ref buffer, size);
                DataReceivedEvent?.Invoke(Encoding.UTF8.GetString(buffer));
              }
            }
            else {
              Logging.Instance().Log(new LogMessage($"Close Tcp connection ({client.Client.RemoteEndPoint})"));

              client.Close();
              clientsToRemove.Add(client);
            }
          }

          if (clientsToRemove.Any()) {
            Clients.RemoveAll(c => clientsToRemove.Contains(c));
          }
        }
      }

      catch (Exception e) {
        Logging.Instance().Log(new LogMessage($"Exception in Run() {e.Message}"));
      }
    }

    public bool Write(string data) {
      lock (_syncObject) {
        foreach (var client in Clients) {
          if (client.GetState() == System.Net.NetworkInformation.TcpState.Established) {
            var stream = client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(data));
          }
        }
      }

      return true;
    }

    public bool Receive(out string data) {
      throw new NotImplementedException();
    }

    protected override void Dispose(bool disposing) {
      base.Dispose(disposing);

      foreach (var client in Clients) {
        client.Close();
      }
      Server.Stop();
    }
  }
}