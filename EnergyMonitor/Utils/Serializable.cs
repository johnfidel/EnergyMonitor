using System;
using System.IO;
using Newtonsoft.Json;

namespace EnergyMonitor.Utils {
  public class Serializable {
    [JsonIgnore]
    public string FileName { get; protected set; }

    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public bool Serialize() {
      if (FileName != "") {
        File.WriteAllText(FileName, ToJson());
        return true;
      }
      return false;
    }

    public static T FromJson<T>(string json)
    where T : new() {
      return JsonConvert.DeserializeObject<T>(json);
    }

    public static T FromFile<T>(string path)
      where T : new() {
      if (File.Exists(path)) {
        return FromJson<T>(File.ReadAllText(path));
      }
      return new T();
    }
  }
}