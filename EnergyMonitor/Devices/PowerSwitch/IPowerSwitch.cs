namespace EnergyMonitor.Devices.PowerSwitch {
  interface IPowerSwitch {
    bool Connected { get; }
    string Ip { get; }
    double Power { get; }
    bool Relay { get; }
    double Temperature { get; }
  }
}