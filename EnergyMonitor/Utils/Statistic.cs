using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnergyMonitor.Utils
{
  public class Entry
  {
    public DateTime TimeStamp { get; set; }
    public double CurrentPower { get; set; }
    public double PhaseAPower { get; set; }
    public double PhaseBPower { get; set; }
    public double PhaseCPower { get; set; }
  }

  public class Statistic : List<Entry>
  {
    public Statistic()
    {
      Add(new Entry());
    }

    public bool Save() {
      var result = false;
      var bulder = new StringBuilder();

      foreach (var entry in this) {
        bulder.AppendLine($"{entry.TimeStamp.ToString("yyyy.MM.dd")};{entry.CurrentPower};{entry.PhaseAPower};{entry.PhaseBPower};{entry.PhaseCPower}");
      }
      File.WriteAllText("statistic.csv", bulder.ToString());

      return result;
    }
  }
}