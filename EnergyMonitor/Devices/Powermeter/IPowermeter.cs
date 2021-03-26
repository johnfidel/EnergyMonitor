using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Types;

namespace EnergyMonitor.Devices.PowerMeter {
  public interface IPowermeter {
    double ActualPowerTotal { get; }
    bool Connected { get; }
    string Ip { get; }
    Phase Phase1 { get; }
    Phase Phase2 { get; }
    Phase Phase3 { get; }
    OutputState RelayState { get; }

    string PrintCurrentValues();
    void SetRelayState(OutputState value);
  }
}