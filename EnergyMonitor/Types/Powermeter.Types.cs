using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Types {
  public enum OutputState {
    Unknown = 0,
    On = 1,
    Off = 2,
  }

  public enum PowermeterType {
    None = 0,
    Shelly3EM
  }

  public class PhaseInfo {
    public double Voltage { get; set; }
    public double Current { get; set; }
    public double Power { get; set; }
    public double Powerfactor { get; set; }
    public double TotalPower { get; set; }
  }

  public class PowerInfo {
    DateTime TimeStamp { get; set; }
    PhaseInfo A { get; set; }
    PhaseInfo B { get; set; }
    PhaseInfo C { get; set; }
    public double TotalPower { get => A.Power + B.Power + C.Power; }
    public double TotalCurrent { get => A.Current + B.Current + C.Current; }
  }
}
