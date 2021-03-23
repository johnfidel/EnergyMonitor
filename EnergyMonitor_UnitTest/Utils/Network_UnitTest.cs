using System.Net;
using System.Net.NetworkInformation;
using System.ComponentModel.DataAnnotations;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils
{
  [TestClass]
  public class Network_UnitTest
  {

    [TestMethod]
    public void Ping_LocalHost_DoesWork()
    {
      Assert.IsTrue(Network.Ping("127.0.0.1"));
      Assert.IsFalse(Network.Ping("10.1.20.20"));
    }

  }
}