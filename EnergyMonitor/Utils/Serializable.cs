using Newtonsoft.Json;

namespace EnergyMonitor.Utils
{
  public class Serializable
  {
    public string ToJson()    
    {
      return JsonConvert.SerializeObject(this);
    }

    public static T FromJson<T>(string json) 
    where T: new()
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
  }
}