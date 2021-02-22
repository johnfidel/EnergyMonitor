using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils {
    [TestClass]
    public class Averager_UnitTest {
        public Averager Avg { get; private set; }

        [TestInitialize]
        public void Setup() {
            Avg = new Averager(10);  
        }

        [TestMethod]
        public void GetAverage_Pass5Times2_Returns2() {
            for (int i = 0; i < 5; i++) {
                Avg.Add(2);
            }
            Assert.AreEqual(2, Avg.GetAverage());
        }

        [TestMethod]
        public void GetAverage_Pass10Times2_Returns2() {
            for (int i = 0; i < 10; i++) {
                Avg.Add(2);
            }
            Assert.AreEqual(2, Avg.GetAverage());
        }

        [TestMethod]
        public void GetAverage_PassAllNumbersFrom1To10_Returns5() {
            for (int i = 1; i <= 10; i++) {
                Avg.Add(i);
            }
            Assert.AreEqual(5.5, Avg.GetAverage());
        }

        [TestMethod]
        public void GetSummary_PassAllNumbersFrom1To10_Returns5() {
            for (int i = 1; i <= 10; i++) {
                Avg.Add(i);
            }
            Assert.AreEqual(55, Avg.GetSummary());
        }

        [TestMethod]
        public void Add_MoreThanCapacity_DoesNotCrash() {            
            for (int i = 1; i <= 20; i++) {
                Avg.Add(i);
            }
        }
    }
}