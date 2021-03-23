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
    public Configuration Configuration { get; set; }
    public State CurrentState { get; private set; }

    protected override void Run()
    {
      if (!Shelly.Connected) {
        Logging.Instance().Log(new LogMessage("No Shelly3EM Device connected"));
        Terminate = true;
      }

      Averager.Add(DateTime.Now, Shelly.ActualPowerTotal);
      var average = Averager.GetAverage();
      CurrentState.ActualAveragePower = average;

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

      Shelly = new Shelly3EM(Configuration.Shelly3EM.IpAddress);
      Averager = new AveragerOverTime(new TimeSpan(0, Configuration.AverageTimeMinutes, 0), false);
      Cycle = Configuration.LogicUpdateRateSeconds * 1000;

      Start();
    }
  }
}