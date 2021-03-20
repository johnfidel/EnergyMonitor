using System;
using System.IO;

namespace EnergyMonitor.Utils
{
  public class Logging
  {
    private string FILE_PATH = "logfile.txt";

    private static Logging _instance;

    private string GetLogfileName()
    {
      return $"{DateTime.Now.Date.ToString("yyyyMMdd")}_{FILE_PATH}";
    }

    public static Logging Instance()
    {
      if (_instance == null)
      {
        _instance = new Logging();
      }
      return _instance;
    }

    public void Log(LogMessage msg)
    {
      File.WriteAllText(GetLogfileName(), msg.ToString());
    }
  }
}