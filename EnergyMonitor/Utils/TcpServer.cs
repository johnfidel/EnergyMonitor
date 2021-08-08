using System.Text;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace EnergyMonitor.Utils {
  public class TcpServer : TaskBase {

    private List<TcpClient> Clients { get; set; }

    private TcpListener Server { get; set; }

    public TcpServer(Int32 listeningPort) : base(100, true) {
      IPAddress localAddr = IPAddress.Parse("127.0.0.1");

      Clients = new List<TcpClient>();

      Server = new TcpListener(localAddr, listeningPort);

      // Start listening for client requests.
      Server.Start();

      Logging.Instance().Log(new LogMessage("TcpServer started"));
    }

    protected override void Run() {
      Logging.Instance().Log(new LogMessage($"Wait for Tcp connection..."));
      var client = Server.AcceptTcpClient();
      Clients.Add(client);
      Logging.Instance().Log(new LogMessage($"Accepted Tcp connection ({client.Client.RemoteEndPoint})"));
    }

    public void SendToClients(string data) {
      List<TcpClient> clientsToRemove = new List<TcpClient>();

      foreach (var client in Clients) {
        if (client.GetState() == System.Net.NetworkInformation.TcpState.Established) {
          var stream = client.GetStream();
          stream.Write(Encoding.ASCII.GetBytes(data));
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

    protected override void Dispose(bool disposing) {
      base.Dispose(disposing);

      foreach (var client in Clients) {
        client.Close();
      }
      Server.Stop();
    }
  }
}