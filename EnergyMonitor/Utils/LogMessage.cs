using System;
namespace EnergyMonitor.Utils
{
  public class LogMessage
  {
    public LogMessage(string msg)
    {
      Date = DateTime.Now;
      Msg = msg;
    }

    public DateTime Date { get; }
    public string Msg { get; }

    public override string ToString()
    {
      return $"{Date.ToString()} {Msg}";
    }
  }
}