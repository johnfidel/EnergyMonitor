using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace EnergyMonitor_UnitTest.Utils {
  [TestClass]
  public class Statistic_UnitTest {
    public Statistic Stat { get; private set; }

    public TestContext TestContext { get; set; }

    private Entry CreateRandomEntry(DateTime timeStamp) {
      var rand = new Random();
      return new Entry {
        CurrentAveragePower = rand.NextDouble(),
        CurrentPower = rand.NextDouble(),
        PhaseAPower = rand.NextDouble(),
        PhaseBPower = rand.NextDouble(),
        PhaseCPower = rand.NextDouble(),
        TimeStamp = timeStamp
      };
    }
    
    [TestInitialize]
    public void Setup() {
      Stat = new Statistic(true);
    }

    [TestMethod]
    public void ReorderStatistic_PreparedList_DoesOrderAsExpected() {
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 8, 6, 42, 0)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 8)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 8)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 9)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 9)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 9)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 10)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 10)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 11)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 12)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 12)));

      Stat.ReorderStatistic();

      Assert.AreEqual(5, Stat.Days.Count);
      Assert.AreEqual(3, Stat.Days[new DateTime(2021, 3, 8)].Count);

      Stat.Save(TestContext.TestResultsDirectory);
      Assert.AreEqual(5, Directory.GetFiles(TestContext.TestResultsDirectory, "*.csv").Length);
    }
  }
}