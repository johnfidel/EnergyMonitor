using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerSwitch.myStrom {
  class NullPowerSwitch : IPowerSwitch {
    public bool Connected => false;

    public string Ip => "";

    public double Power => 0;

    public bool Relay => false;

    public double Temperature => 0;
  }
}
