using System.Net;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;
using System.IO;
using static EnergyMonitor.Devices.PowerMeter.Shelly.Shelly3EM;
using EnergyMonitor.Types;
using EnergyMonitor.Devices.PowerMeter;
using EnergyMonitor.Devices.PowerSwitch;

namespace EnergyMonitor.BusinessLogic {
  public class Logic : TaskBase {

    private IPowermeter Powermeter { get; set; }
    private IPowerSwitch PowerSwitch { get; set; }
    private AveragerOverTime Averager { get; set; }

    protected virtual DateTime TimeSource { get => DateTime.Now; }

    public Configuration Configuration { get; protected set; }
    public Statistic Statistic { get; private set; }
    public State CurrentState { get; private set; }

    private bool LockingTimeRangeSet() {
      return (Configuration.LockTimeStart != new DateTime() && Configuration.LockTimeEnd != new DateTime());
    }

    protected bool IsLocked() {

      if (LockingTimeRangeSet() &&
        (TimeSource.TimeOfDay >= Configuration.LockTimeStart.TimeOfDay && 
        TimeSource.TimeOfDay <= Configuration.LockTimeEnd.TimeOfDay)) {
        return true;
      }
      return false;
    }

    protected bool IsLockedConsiderSwitchOffDelay() {
      if (LockingTimeRangeSet() &&
        (TimeSource.TimeOfDay + new TimeSpan(0, Configuration.ForceSwitchOffDelayMinutes, 0) > Configuration.LockTimeStart.TimeOfDay) &&
        (TimeSource.TimeOfDay < Configuration.LockTimeEnd.TimeOfDay) ||
        IsLocked()) {
        return true;
      }
      return false;
    }

    protected void MoveMeasuresToState(double average) {
      CurrentState.ActualAveragePower = Math.Round(average, 3);
      CurrentState.CurrentPower = Math.Round(Powermeter.ActualPowerTotal, 3);
      CurrentState.CurrentPhaseAPower = Math.Round(Powermeter.Phase1.Power, 3);
      CurrentState.CurrentPhaseBPower = Math.Round(Powermeter.Phase2.Power, 3);
      CurrentState.CurrentPhaseCPower = Math.Round(Powermeter.Phase3.Power, 3);
      CurrentState.SolarPower = Math.Round(PowerSwitch.Power, 3);
    }

    protected void AddStatisticEntry(double average) {
      Statistic.Add(new Entry {
        CurrentPower = Powermeter.ActualPowerTotal,
        CurrentAveragePower = average,
        PhaseAPower = Powermeter.Phase1.Power,
        PhaseBPower = Powermeter.Phase2.Power,
        PhaseCPower = Powermeter.Phase3.Power,
        SolarPower = PowerSwitch.Power,
        TimeStamp = DateTime.Now
      });
    }

    protected override void Run() {
      if (!Powermeter.Connected) {
        Logging.Instance().Log(new LogMessage("Could not connect to Shelly"));
        Terminate = true;
      }

      if (!Powermeter.Connected) {
        Logging.Instance().Log(new LogMessage("Could not connect to Powermeter"));
        Terminate = true;
      }

      // reload configuration
      Configuration = Serializable.FromJson<Configuration>(File.ReadAllText(Configuration.CONFIG_FILE_NAME));

      Averager.Add(DateTime.Now, Powermeter.ActualPowerTotal);      
      var average = Averager.GetAverage();
      MoveMeasuresToState(average);

      AddStatisticEntry(average);

      if (!IsLockedConsiderSwitchOffDelay()) {
        CurrentState.Locked = false;
        if (average > Configuration.OffThreshold) {
          CurrentState.ActualOutputState = OutputState.Off;
          Powermeter.SetRelayState(OutputState.Off);
        }
        else if (average < Configuration.OnThreshold) {
          CurrentState.ActualOutputState = OutputState.On;
          Powermeter.SetRelayState(OutputState.On);
        }
      }
      else {
        CurrentState.Locked = true;
        Powermeter.SetRelayState(OutputState.Off);
      }
      CurrentState.Serialize();
    }

    public Logic(bool suspended) : base(100, suspended) {
      Configuration = Configuration.Load();
      CurrentState = Serializable.FromFile<State>(State.FILENAME);
      Statistic = new Statistic(false);
      Powermeter = PowermeterFactory.CreatePowermeter(PowermeterType.Shelly3EM, Configuration.PowerMeter.IpAddress, Simulation.Simulate_PowerMeter());
      PowerSwitch = PowerSwitchFactory.CreateDevice(PowerSwitchType.MyStrom, Configuration.PowerSwitch.IpAddress, Simulation.Simulate_PowerSwitch());
      Averager = new AveragerOverTime(new TimeSpan(0, Configuration.AverageTimeMinutes, Configuration.AverageTimeSeconds));
      Averager.Start();
      Cycle = Configuration.LogicUpdateRateSeconds * 1000;

      if (!suspended) { Start(); }
    }
  }
}