using System.Net;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using EnergyMonitor.Utils;
using EnergyMonitor.Devices.PowerMeter.Shelly;
using System;
using System.IO;

namespace EnergyMonitor.BusinessLogic
{
  public class Logic : TaskBase
  {

    private Shelly3EM Shelly { get; set; }
    private AveragerOverTime Averager { get; set; }
    public Configuration Configuration { get; private set; }
    public Statistic Statistic { get; private set; }
    public State CurrentState { get; private set; }

    protected override void Run()
    {
      if (!Shelly.Connected)
      {
        Logging.Instance().Log(new LogMessage("Could not connect to Shelly"));
        Terminate = true;
      }

      Averager.Add(DateTime.Now, Shelly.ActualPowerTotal);
      Logging.Instance().Log(new LogMessage($"Averager has {Averager.Count} values"));
      var average = Averager.GetAverage();
      CurrentState.ActualAveragePower = Math.Round(average, 3);
      CurrentState.CurrentPower = Math.Round(Shelly.ActualPowerTotal, 3);
      CurrentState.CurrentPhaseAPower = Math.Round(Shelly.Phase1.Power, 3);
      CurrentState.CurrentPhaseBPower = Math.Round(Shelly.Phase2.Power, 3);
      CurrentState.CurrentPhaseCPower = Math.Round(Shelly.Phase3.Power, 3);

      Statistic.Add(new Entry
      {
        CurrentPower = Shelly.ActualPowerTotal,
        PhaseAPower = Shelly.Phase1.Power,
        PhaseBPower = Shelly.Phase2.Power,
        PhaseCPower = Shelly.Phase3.Power,
        TimeStamp = DateTime.Now
      });
      Statistic.Save();

      if (average > Configuration.OffThreshold)
      {
        CurrentState.ActualOutputState = State.OutputState.Off;
        Shelly.SetRelayState(false);
      }
      else if (average < Configuration.OnThreshold)
      {
        CurrentState.ActualOutputState = State.OutputState.On;
        Shelly.SetRelayState(true);
      }
      CurrentState.Serialize();
    }

    public Logic() : base(100, true)
    {
      Configuration = Configuration.Load();
      CurrentState = new State();
      Statistic = new Statistic();
      Shelly = new Shelly3EM(Configuration.Shelly3EM.IpAddress);
      Averager = new AveragerOverTime(new TimeSpan(0, Configuration.AverageTimeMinutes, Configuration.AverageTimeSeconds));
      Averager.Start();
      Cycle = Configuration.LogicUpdateRateSeconds * 1000;

      Start();
    }
  }
}