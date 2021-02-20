using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnergyMonitor.Devices.PowerMeter.Shelly;

namespace EnergyMonitor_UnitTest.Devices.PowerMeter.Shelly {
    [TestClass]
    public class Shelly3EM_UnitTest {

        [TestMethod]
        public void GetEmeterData_RealDevice_Test() {
            var device = new Shelly3EM();
            Thread.Sleep(1000);            
        }
    }
}
