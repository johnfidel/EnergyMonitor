using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerSwitch.myStrom.Types {
  class Report {
    public double power { get; set; }
    public bool relay { get; set; }
    public double temperature { get; set; }
  }
}
