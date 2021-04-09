using System.Text.Json.Serialization;
using EnergyMonitor.Types;
using EnergyMonitor.Utils;
using Newtonsoft.Json.Converters;

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
    public double SolarPower { get; set; }
    public bool Locked { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public OutputState ActualOutputState { get; set; }

    public State()
    {
      FileName = FILENAME;
    }
  }
}