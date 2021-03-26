using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerMeter.Shelly.Simulation {
  class SimulatedShelly3EM : IPowermeter {
    public double ActualPowerTotal => 100;

    public bool Connected => true;

    public string Ip => "192.168.2.22";

    public Phase Phase1 => new Phase { Current = 1, Voltage = 230, Power = 230 };

    public Phase Phase2 => new Phase { Current = 2, Voltage = 231, Power = 642 };

    public Phase Phase3 => new Phase { Current = 3, Voltage = 232, Power = 696 };

    private OutputState _relayState = OutputState.Unknown;
    public OutputState RelayState => _relayState; 

    public string PrintCurrentValues() {
      var builder = new StringBuilder();
      builder.AppendLine(Phase1.ToString())
        .AppendLine(Phase2.ToString())
        .AppendLine(Phase3.ToString())
        .AppendLine($"Total: {ActualPowerTotal}W");

      return builder.ToString();
    }

    public void SetRelayState(OutputState value) {
      _relayState = value;
    }
  }
}
