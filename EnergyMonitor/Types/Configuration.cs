using System;
using System.Net.Sockets;
using System.IO;
using EnergyMonitor.Utils;

namespace EnergyMonitor.Types {
  public class Configuration : Serializable {
    public class NetworkDevice {
      public string IpAddress { get; set; }
    }

    public const string CONFIG_FILE_NAME = "config.json";

    private double DefaultOffThreshold = 280;
    private double DefaultOnThreshold = -250;
    private int DefaultAverageTime = 10;       // in minutes
    private int DefaultLogicUpdateRate = 5;   // in seconds;
    private string DefaultShellyIp = "192.168.2.78";
    private string DefaultMyStromSwitchIp = "192.168.2.17";
    /// <summary>
    /// The timeout delay WP has to runout after force command is removed
    /// </summary>
    private int DefaultForceSwitchOffDelayMinutes = 20; //minutes

    public double OffThreshold { get; set; }
    public double OnThreshold { get; set; }
    public int AverageTimeMinutes { get; set; }
    public int AverageTimeSeconds { get; set; }
    public int LogicUpdateRateSeconds { get; set; }
    public NetworkDevice PowerMeter { get; set; }
    public NetworkDevice PowerSwitch { get; set; }
    public DateTime LockTimeStart { get; set; }
    public DateTime LockTimeEnd { get; set; }
    public int ForceSwitchOffDelayMinutes { get; set; }

    public void Save() {
      File.WriteAllText(CONFIG_FILE_NAME, ToJson());
    }

    public static Configuration Load() {

      if (!File.Exists(CONFIG_FILE_NAME)) {
        new Configuration().Save();
      }

      var config = FromJson<Configuration>(File.ReadAllText(CONFIG_FILE_NAME));
      config.Save();
      return config;
    }

    public Configuration() {
      OffThreshold = DefaultOffThreshold;
      OnThreshold = DefaultOnThreshold;
      AverageTimeMinutes = DefaultAverageTime;
      AverageTimeSeconds = 0;
      LogicUpdateRateSeconds = DefaultLogicUpdateRate;
      PowerMeter = new NetworkDevice {
        IpAddress = DefaultShellyIp
      };
      PowerSwitch = new NetworkDevice {
        IpAddress = DefaultMyStromSwitchIp
      };
      ForceSwitchOffDelayMinutes = DefaultForceSwitchOffDelayMinutes;
    }
  }
}
