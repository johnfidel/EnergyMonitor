using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnergyMonitor.Utils {
  public class Entry {
    public DateTime TimeStamp { get; set; }
    public double CurrentPower { get; set; }
    public double CurrentAveragePower { get; set; }
    public double PhaseAPower { get; set; }
    public double PhaseBPower { get; set; }
    public double PhaseCPower { get; set; }
    public double SolarPower { get; set; }
  }

  public class Day : List<Entry> {
    public bool Saved { get; set; }
    public DateTime Date { get => this.FirstOrDefault().TimeStamp.Date; }
    public Day(List<Entry> entries) {
      AddRange(entries);
    }
    public Day() { Saved = false; }

    public void Save(string rootDirectory) {
      if (!Saved) {
        var builder = new StringBuilder();

        foreach (var entry in this) {
          builder.AppendLine($"{entry.TimeStamp.ToString("yyyy.MM.dd hh:mm.ss")};{entry.CurrentPower};{entry.PhaseAPower};{entry.PhaseBPower};{entry.PhaseCPower};{entry.CurrentAveragePower};{entry.SolarPower}");
        }
        File.WriteAllText(Path.Combine(rootDirectory, $"{Date.ToString("yyyyMMdd")}_statistic.csv"), builder.ToString());

        Saved = true;
      }
    }
  }

  public class Statistic : ConcurrentBag<Entry>, IDisposable {
    private CancellationTokenSource Cancel { get; }
    private Task Worker { get; }
    private object _syncObject;

    public Dictionary<DateTime, Day> Days { get; set; }

    public Statistic(bool noWorker) {
      Days = new Dictionary<DateTime, Day>();
      _syncObject = new object();

      if (!noWorker) {
        Cancel = new CancellationTokenSource();
        Worker = Task.Factory.StartNew(() => {
          while (!Cancel.IsCancellationRequested) {
            Thread.Sleep(1000);
            ReorderStatistic();
          }
        }, Cancel.Token);
      }
    }

    public void Save(string rootDirectory) {
      foreach (var day in Days) {
        day.Value.Save(rootDirectory);
      }
    }

    public void Save() {
      Save(AppDomain.CurrentDomain.BaseDirectory);
    }

    public void ReorderStatistic() {
      lock (_syncObject) {
        while (TryTake(out var item)) {
          if (!Days.TryGetValue(item.TimeStamp.Date, out var day)) {
            Days[item.TimeStamp.Date] = new Day();
          }
          Days[item.TimeStamp.Date].Add(item);
        }
      }
    }

    protected void Dispose(bool disposing) {
      Cancel.Cancel();
      Worker.Wait();
      ReorderStatistic();
      Save();
    }

    public void Dispose() {
      Dispose(false);
    }
  }
}