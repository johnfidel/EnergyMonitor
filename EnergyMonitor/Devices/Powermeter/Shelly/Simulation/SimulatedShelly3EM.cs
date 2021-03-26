using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerMeter.Shelly.Simulation {
  class SimulatedShelly3EM : IPowermeter {
    public double ActualPowerTotal => throw new NotImplementedException();

    public bool Connected => throw new NotImplementedException();

    public string Ip => throw new NotImplementedException();

    public Phase Phase1 => throw new NotImplementedException();

    public Phase Phase2 => throw new NotImplementedException();

    public Phase Phase3 => throw new NotImplementedException();

    public OutputState RelayState => throw new NotImplementedException();

    public string PrintCurrentValues() {
      throw new NotImplementedException();
    }

    public void SetRelayState(OutputState value) {
      throw new NotImplementedException();
    }
  }
}
