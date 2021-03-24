using System.Threading;
using System.Transactions;
using System;
using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnergyMonitor_UnitTest.Utils {

  class AverageOverTimeAccessor : AveragerOverTime {
    public AverageOverTimeAccessor(TimeSpan span) : base(span) { }
    public DateTime CurrentTime { get; set; }
    protected override DateTime TimeSource => CurrentTime;

    [TestClass]
    public class AveragerOverTime_UnitTest {
      public AverageOverTimeAccessor Avg { get; private set; }

      [TestInitialize]
      public void Setup() {
        // measures valid for 5 seconds
        Avg = new AverageOverTimeAccessor(new System.TimeSpan(0, 0, 5));
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 01);
      }

      [TestMethod]
      public void GetSummary_Pass5Times2_Wait2Seconds() {
        var baseTime = new DateTime(2020, 01, 01, 01, 01, 01);
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

      [TestMethod]
      public void UpdateValues_CurrentTimeChanges_DoesRemoveOldEntries() {
        var baseTime = new DateTime(2020, 01, 01, 01, 01, 01);
        Avg.Add(baseTime, 5);
        Avg.Add(baseTime.AddSeconds(1), 5);
        Avg.Add(baseTime.AddSeconds(2), 5);
        Avg.Add(baseTime.AddSeconds(3), 5);
        Avg.Add(baseTime.AddSeconds(4), 5);

        Assert.AreEqual(5, Avg.Count);
        
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 7);
        Avg.UpdateValues();
        Assert.AreEqual(4, Avg.Count);
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 8);
        Avg.UpdateValues();
        Assert.AreEqual(3, Avg.Count);
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 9);
        Avg.UpdateValues();
        Assert.AreEqual(2, Avg.Count);
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 12);
        Avg.UpdateValues();
        Assert.AreEqual(0, Avg.Count);
      }

      [TestMethod]
      public void GetAverage_DummyValues_DoesWork() {
        var baseTime = new DateTime(2020, 01, 01, 01, 01, 01);
        Avg.Add(baseTime, 5);
        Avg.Add(baseTime.AddSeconds(1), 5);
        Avg.Add(baseTime.AddSeconds(2), 5);
        Avg.Add(baseTime.AddSeconds(3), 5);
        Avg.Add(baseTime.AddSeconds(4), 5);

        Assert.AreEqual(5, Avg.GetAverage());
        Avg.CurrentTime = new DateTime(2020, 01, 01, 01, 01, 7);
        Avg.UpdateValues();
        Assert.AreEqual(5, Avg.GetAverage());
        Avg.Add(baseTime.AddSeconds(5), 1);
        // 5 5 5 5 1 = 21 => avg = 4.2
        Assert.AreEqual(4.2, Avg.GetAverage());
      }

      [TestMethod]
      public void GetAverage_Real() {
        var avg = new AveragerOverTime(new TimeSpan(0, 0, 5));
        avg.Start();
        var time = DateTime.Now;
        avg.Add(time, 5);
        avg.Add(time.AddSeconds(1), 5);
        avg.Add(time.AddSeconds(2), 5);
        Assert.AreEqual(5, avg.GetAverage());
        avg.Add(time.AddSeconds(10), 1);
        Thread.Sleep(8000);
        Assert.AreEqual(1, avg.GetAverage());
      }
    }
  }
}