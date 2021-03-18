using System.Text.Json.Serialization;
using EnergyMonitor.Utils;
using Newtonsoft.Json.Converters;

namespace EnergyMonitor.BusinessLogic
{
  public class State : Serializable
  {
    private string FILENAME = "state.info";

    public enum OutputState
    {
      Unknown = 0,
      On = 1,
      Off = 2,
    }

    public double ActualAveragePower { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]    
    public OutputState ActualOutputState { get; set; }

    public State() {
      FileName = FILENAME;
    }
  }
}