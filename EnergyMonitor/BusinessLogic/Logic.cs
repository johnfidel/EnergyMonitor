using System.Net;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;
using System.IO;
using static EnergyMonitor.Devices.PowerMeter.Shelly.Shelly3EM;
using EnergyMonitor.Types;

namespace EnergyMonitor.BusinessLogic {
  public class Logic : TaskBase {

    private Shelly3EM Shelly { get; set; }
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
        (TimeSource >= Configuration.LockTimeStart && TimeSource <= Configuration.LockTimeEnd)) {
        return true;
      }
      return false;
    }

    protected bool IsLockedConsiderSwitchOffDelay() {
      if (LockingTimeRangeSet() &&
        (TimeSource + new TimeSpan(0, Configuration.ForceSwitchOffDelayMinutes, 0) > Configuration.LockTimeStart) &&
        (TimeSource < Configuration.LockTimeEnd) ||
        IsLocked()) {
        return true;
      }
      return false;
    }

    protected void MoveMeasuresToState(double average) {
      CurrentState.ActualAveragePower = Math.Round(average, 3);
      CurrentState.CurrentPower = Math.Round(Shelly.ActualPowerTotal, 3);
      CurrentState.CurrentPhaseAPower = Math.Round(Shelly.Phase1.Power, 3);
      CurrentState.CurrentPhaseBPower = Math.Round(Shelly.Phase2.Power, 3);
      CurrentState.CurrentPhaseCPower = Math.Round(Shelly.Phase3.Power, 3);
    }

    protected void AddStatisticEntry(double average) {
      Statistic.Add(new Entry {
        CurrentPower = Shelly.ActualPowerTotal,
        CurrentAveragePower = average,
        PhaseAPower = Shelly.Phase1.Power,
        PhaseBPower = Shelly.Phase2.Power,
        PhaseCPower = Shelly.Phase3.Power,
        TimeStamp = DateTime.Now
      });
      Statistic.Save();
    }

    protected override void Run() {
      if (!Shelly.Connected) {
        Logging.Instance().Log(new LogMessage("Could not connect to Shelly"));
        Terminate = true;
      }

      // reload configuration
      Configuration = Serializable.FromJson<Configuration>(File.ReadAllText(Configuration.CONFIG_FILE_NAME));

      Averager.Add(DateTime.Now, Shelly.ActualPowerTotal);      
      var average = Averager.GetAverage();
      MoveMeasuresToState(average);

      AddStatisticEntry(average);

      if (!IsLockedConsiderSwitchOffDelay()) {
        CurrentState.Locked = false;
        if (average > Configuration.OffThreshold) {
          CurrentState.ActualOutputState = OutputState.Off;
          Shelly.SetRelayState(OutputState.Off);
        }
        else if (average < Configuration.OnThreshold) {
          CurrentState.ActualOutputState = OutputState.On;
          Shelly.SetRelayState(OutputState.On);
        }
      }
      else {
        CurrentState.Locked = true;
        Shelly.SetRelayState(OutputState.Off);
      }
      CurrentState.Serialize();
    }

    protected Logic(bool suspended) : base(100, suspended) {
      Configuration = Configuration.Load();
      CurrentState = Serializable.FromFile<State>(State.FILENAME);
      Statistic = new Statistic();
      Shelly = new Shelly3EM(Configuration.Shelly3EM.IpAddress);
      Averager = new AveragerOverTime(new TimeSpan(0, Configuration.AverageTimeMinutes, Configuration.AverageTimeSeconds));
      Averager.Start();
      Cycle = Configuration.LogicUpdateRateSeconds * 1000;

      if (!suspended) { Start(); }
    }

    public Logic() : this(false) { }
  }
}