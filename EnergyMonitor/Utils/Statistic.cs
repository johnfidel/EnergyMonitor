using System;
using System.Collections.Generic;

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
  }
}