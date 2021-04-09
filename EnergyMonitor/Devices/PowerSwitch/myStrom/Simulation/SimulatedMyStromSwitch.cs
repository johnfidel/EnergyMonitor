using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerSwitch.myStrom.Simulation {
  class SimulatedMyStromSwitch : IPowerSwitch {
    public bool Connected => true;

    public string Ip => "192.168.2.33";

    public double Power => 53;

    public bool Relay => true;

    public double Temperature => 21;
  }
}
