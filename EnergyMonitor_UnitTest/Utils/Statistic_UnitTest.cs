using EnergyMonitor.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace EnergyMonitor_UnitTest.Utils {

  class StatisticAccessor : Statistic {
    public StatisticAccessor() : base(true) {
    }

    public DateTime CurrentTime { get; set; }
    protected override DateTime TimeSource => CurrentTime;
  }

  [TestClass]
  public class Statistic_UnitTest {
    internal StatisticAccessor Stat { get; private set; }

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
      Stat = new StatisticAccessor();
    }

    [TestMethod]
    public void ReorderStatistic_PreparedList_DoesOrderAsExpected() {
      Stat.CurrentTime = new DateTime(2021, 4, 1);
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

      Stat.SeparatePastDays();

      Assert.AreEqual(5, Stat.Days.Count);
      Assert.AreEqual(3, Stat.Days[new DateTime(2021, 3, 8)].Count);

      Stat.Save(TestContext.TestResultsDirectory);
      Assert.AreEqual(5, Directory.GetFiles(TestContext.TestResultsDirectory, "*.csv").Length);
    }

    [TestMethod]
    public void SpearatePastDays_OnlyCreateEntriesForPastDays_DoesWork() {
      Stat.CurrentTime = new DateTime(2021, 3, 10);
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 8)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 8)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 9)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 9)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 10)));
      Stat.Add(CreateRandomEntry(new DateTime(2021, 3, 10)));

      Stat.SeparatePastDays();
      Assert.AreEqual(2, Stat.Days.Count);
    }

    [TestMethod]
    public void CondensateDays_PreloadData_DoesCondensate() {
      Stat.CurrentTime = new DateTime(2021, 3, 5);

      for (int day = 1; day < 5; day++) {
        for (int hour = 0; hour < 24; hour++) {
          for (int minute = 0; minute < 60; minute++) {
            for (int i = 0; i < 60; i += 5) {
              Stat.Add(new Entry {
                TimeStamp = new DateTime(2021, 3, day, hour, minute, i),
                PhaseAPower = 100,
                PhaseBPower = 200,
                PhaseCPower = 300,
                SolarPower = 150
              });
            }
          }
        }
      }

      Stat.SeparatePastDays();
      Stat.CondensateDays();
    }
  }
}