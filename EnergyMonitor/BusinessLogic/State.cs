using System.Text.Json.Serialization;
using EnergyMonitor.Utils;
using Newtonsoft.Json.Converters;
using static EnergyMonitor.Devices.PowerMeter.Shelly.Shelly3EM;

namespace EnergyMonitor.BusinessLogic
{
  public class State : Serializable
  {
    public const string FILENAME = "state.info";

    public double ActualAveragePower { get; set; }
    public double CurrentPower { get; set; }

    public double CurrentPhaseAPower { get; set; }
    public double CurrentPhaseBPower { get; set; }
    public double CurrentPhaseCPower { get; set; }
    public bool Locked { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public OutputState ActualOutputState { get; set; }

    public State()
    {
      FileName = FILENAME;
    }
  }
}