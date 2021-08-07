using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel.DataAnnotations;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils
{
  [TestClass]
  public class TcpServer_UnitTest
  {
    public TcpServer Server { get; private set; }

    [TestInitialize]
    public void Setup() {
      Server = new TcpServer(55000);
      Server.Start();
    }

    [TestMethod]
    public void ReconnectIsPossible()
    {
      var client = new TcpClient();
      var IpAdress = IPAddress.Parse("127.0.0.1");
      client.Connect(IpAdress, 55000);
      Server.SendToClients("test");
      client.Client.Close();      
      Server.SendToClients("test");
    }

  }
}