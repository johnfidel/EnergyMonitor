using System.Threading;
using System.Transactions;
using System;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils
{

    class AverageOverTimeAccessor : AveragerOverTime {
        public AverageOverTimeAccessor(TimeSpan span) : base(span, true) { }
        public DateTime CurrentTime {get;set;}
        protected override DateTime TimeSource => CurrentTime;

        [TestClass]
        public class AveragerOverTime_UnitTest {
            public AverageOverTimeAccessor Avg { get; private set; }

            [TestInitialize]
            public void Setup() {
                // measures valid for 5 seconds
                Avg = new AverageOverTimeAccessor(new System.TimeSpan(0,0,5));
                Avg.CurrentTime = new DateTime(2020,01,01,01,01,01);
            }

            [TestMethod]
            public void GetSummary_Pass5Times2_Wait2Seconds() {
                var baseTime = new DateTime(2020,01,01,01,01,01);
                Avg.Add(baseTime, 1);
                Avg.Add(baseTime.AddSeconds(1), 2);
                Avg.Add(baseTime.AddSeconds(2), 3);
                Avg.Add(baseTime.AddSeconds(3), 4);
                Avg.Add(baseTime.AddSeconds(4), 5);

                Assert.AreEqual(3, Avg.GetAverage());

                Avg.CurrentTime = Avg.CurrentTime.AddSeconds(6);
                Avg.UpdateValues();
                Assert.AreEqual(3.5, Avg.GetAverage());
                
                Avg.CurrentTime = Avg.CurrentTime.AddSeconds(1);
                Avg.UpdateValues();
                Assert.AreEqual(4, Avg.GetAverage());
            }
        }
    }
}