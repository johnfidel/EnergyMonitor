using System.Net.Sockets;
using System.IO;
using EnergyMonitor.Utils;

namespace EnergyMonitor.BusinessLogic
{
  public class Configuration : Serializable
  {
    private const string CONFIG_FILE_NAME = "config.json";

    private double DefaultOverpowerThreshold = 350;
    private double DefaultUnderpowerThreshold = -250;
    private int DefaultAverageTime = 2;       // in minutes
    private int DefaultLogicUpdateRate = 5;   // in seconds;


    public double OverpowerThreshold { get; set; }
    public double UnderpowerThredhols { get; set; }
    public int AverageTimeMinutes { get; set; }
    public int LogicUpdateRateSeconds {get;set;}

    public void Save()
    {
      File.WriteAllText(CONFIG_FILE_NAME, ToJson());
    }

    public static Configuration Load()
    {

      if (!File.Exists(CONFIG_FILE_NAME))
      {
        new Configuration().Save();
      }

      return FromJson<Configuration>(File.ReadAllText(CONFIG_FILE_NAME));
    }

    public Configuration() {
      OverpowerThreshold = DefaultOverpowerThreshold;
      UnderpowerThredhols = DefaultUnderpowerThreshold;
      AverageTimeMinutes = DefaultAverageTime;       
      LogicUpdateRateSeconds = DefaultLogicUpdateRate;
    }
  }
}
