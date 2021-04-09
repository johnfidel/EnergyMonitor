using EnergyMonitor.Devices.PowerSwitch.myStrom;
using EnergyMonitor.Devices.PowerSwitch.myStrom.Simulation;
using EnergyMonitor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerSwitch {
  static class PowerSwitchFactory {
    public static IPowerSwitch CreateDevice(PowerSwitchType type, string ip, bool simulated) {
      switch (type) {
        case PowerSwitchType.MyStrom: {
          if (simulated) {
            return new SimulatedMyStromSwitch();
          }
          else {
            return new MyStromSwitch(ip);
          }
        }

        default: {
          return new NullPowerSwitch();
        }
      }
    }
  }
}
