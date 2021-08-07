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

    public TcpServer(Int32 listeningPort) : base() {
      IPAddress localAddr = IPAddress.Parse("127.0.0.1");

      Clients = new List<TcpClient>();

      Server = new TcpListener(localAddr, listeningPort);

      // Start listening for client requests.
      Server.Start();
    }

    protected override void Run() {
      Clients.Add(Server.AcceptTcpClient());
    }

    public void SendToClients(string data) {
      List<TcpClient> clientsToRemove = new List<TcpClient>();

      foreach (var client in Clients) {
        if (client.GetState() == System.Net.NetworkInformation.TcpState.Established) {
          var stream = client.GetStream();
          stream.Write(Encoding.ASCII.GetBytes(data));
        }
        else {
          client.Close();
          clientsToRemove.Add(client);
        }        
      }

      if (clientsToRemove.Any()) {        
        Clients.RemoveAll(c => clientsToRemove.Contains(c));
      }
    }
  }
}