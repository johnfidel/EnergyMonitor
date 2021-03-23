using System;
using System.Net.Sockets;
using System.IO;
using EnergyMonitor.Utils;

namespace EnergyMonitor.BusinessLogic
{
  public class Configuration : Serializable
  {
    public class NetworkDevice
    {
      public string IpAddress { get; set; }
    }

    private const string CONFIG_FILE_NAME = "config.json";

    private double DefaultOffThreshold = 350;
    private double DefaultOnThreshold = -250;
    private int DefaultAverageTime = 2;       // in minutes
    private int DefaultLogicUpdateRate = 5;   // in seconds;
    private string DefaultShellyIp = "192.168.2.78";


    public double OffThreshold { get; set; }
    public double OnThreshold { get; set; }
    public int AverageTimeMinutes { get; set; }
    public int AverageTimeSeconds { get; set; }
    public int LogicUpdateRateSeconds { get; set; }
    public NetworkDevice Shelly3EM { get; set; }

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

    public Configuration()
    {
      OffThreshold = DefaultOffThreshold;
      OnThreshold = DefaultOnThreshold;
      AverageTimeMinutes = DefaultAverageTime;
      AverageTimeSeconds = 0;
      LogicUpdateRateSeconds = DefaultLogicUpdateRate;
      Shelly3EM = new NetworkDevice
      {
        IpAddress = DefaultShellyIp
      };
    }
  }
}
