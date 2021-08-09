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
    public bool Condensated { get; set; }

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

  public class Statistic : Serializable, IDisposable {
    public static readonly string FILENAME = "tempStatistic.json";
    private CancellationTokenSource Cancel { get; }
    private Task Worker { get; }
    private object _syncObject;
    protected virtual DateTime TimeSource { get => DateTime.Now; }
    public List<Entry> Data { get; set; }

    public Dictionary<DateTime, Day> Days { get; set; }

    public Statistic(bool noWorker) {
      FileName = FILENAME;
      Data = new List<Entry>();
      Days = new Dictionary<DateTime, Day>();
      _syncObject = new object();

      if (!noWorker) {
        Cancel = new CancellationTokenSource();
        Worker = Task.Factory.StartNew(() => {
          while (!Cancel.IsCancellationRequested) {
            Thread.Sleep(1000);
            SeparatePastDays();
            Serialize();
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

    public void SeparatePastDays() {
      lock (_syncObject) {
        // only take items from yesterday
        var yesterdayItems = Data.Where(temp => TimeSource.Date.AddDays(-1) >= temp.TimeStamp.Date).ToList();
        // remove taken elements
        Data.RemoveAll(e => yesterdayItems.Contains(e));
        // order them into correct day elements
        foreach (var item in yesterdayItems) {
          if (TimeSource.Date.AddDays(-1) >= item.TimeStamp.Date) {
            if (!Days.TryGetValue(item.TimeStamp.Date, out var day)) {
              Days[item.TimeStamp.Date] = new Day();
            }
            Days[item.TimeStamp.Date].Add(item);
          }
        }
      }
    }

    public void CondensateDays() {
      lock (_syncObject) {
        List<Day> condensatedDays = new List<Day>();
        DateTime last = new DateTime();
        foreach (var day in Days) {
          if (!day.Value.Condensated) {
            var condensatedDay = new Day();
            var condensatedEntry = new Entry();
            foreach (var entry in day.Value) {

              // save only one point per hour, integrate power over whole day
              if (last != new DateTime()) {
                var timeDiff = entry.TimeStamp - last;
                // integrate time
                condensatedEntry.TimeStamp = new DateTime(entry.TimeStamp.Year, entry.TimeStamp.Month, entry.TimeStamp.Day, entry.TimeStamp.Hour, 0, 0);
                condensatedEntry.PhaseAPower += (entry.PhaseAPower * timeDiff.TotalSeconds);
                condensatedEntry.PhaseBPower += (entry.PhaseBPower * timeDiff.TotalSeconds);
                condensatedEntry.PhaseCPower += (entry.PhaseCPower * timeDiff.TotalSeconds);
                condensatedEntry.SolarPower += (entry.SolarPower * timeDiff.TotalSeconds);
                condensatedEntry.CurrentAveragePower += (entry.CurrentAveragePower * timeDiff.TotalSeconds);
              }

              if (entry.TimeStamp.Hour > last.Hour) {
                condensatedDay.Add(entry);
                condensatedEntry = new Entry();
              }

              last = entry.TimeStamp;
            }
            condensatedDay.Condensated = true;
            condensatedDays.Add(condensatedDay);
          }
        }
        Days.Clear();

        // add new days 
        foreach (var day in condensatedDays) {
          Days.TryAdd(day.Date, day);
        }
      }
    }

    protected void Dispose(bool disposing) {
      Cancel.Cancel();
      Worker.Wait();
      SeparatePastDays();
      Save();
    }

    public void Dispose() {
      Dispose(false);
    }

    public void Add(Entry entry) {
      Data.Add(entry);
    }
  }
}