using EnergyMonitor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerMeter {
  static class PowermeterFactory {
    public static IPowermeter CreatePowermeter(PowermeterType type, string Ip, bool simulated) {
      switch (type) {
        case PowermeterType.Shelly3EM: {
          if (simulated) {
            return new Shelly.Simulation.SimulatedShelly3EM();
          }
          else {
            return new Shelly.Shelly3EM(Ip);
          }
        }

        default: {
          return new NullPowermeter();
        }
      }
    }
  }
}
