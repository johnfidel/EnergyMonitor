using System.Net.Sockets;
using System.IO;
using EnergyMonitor.Utils;

namespace EnergyMonitor.BusinessLogic
{
  public class Configuration : Serializable
  {
    private const string CONFIG_FILE_NAME = "config.json";

    public double OnThreshold { get; set; }
    public double OffThreshold { get; set; }

    public void Save() {
      File.WriteAllText(CONFIG_FILE_NAME, ToJson());
    }

    public static Configuration Load() {

      if (!File.Exists(CONFIG_FILE_NAME)) {
        new Configuration().Save();
      }

      return FromJson<Configuration>(File.ReadAllText(CONFIG_FILE_NAME));
    }
  }
}
