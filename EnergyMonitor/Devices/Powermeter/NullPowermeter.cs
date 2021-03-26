﻿using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerMeter {
  class NullPowermeter : IPowermeter {
    public double ActualPowerTotal => throw new NotImplementedException();

    public bool Connected => throw new NotImplementedException();

    public string Ip => throw new NotImplementedException();

    public Phase Phase1 => new Phase();

    public Phase Phase2 => new Phase();

    public Phase Phase3 => new Phase();

    public OutputState RelayState => OutputState.Unknown;

    public string PrintCurrentValues() {
      return "";
    }

    public void SetRelayState(OutputState value) { }
  }
}