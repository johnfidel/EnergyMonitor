using System.Diagnostics.Tracing;
using System.Text;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using EnergyMonitor.Devices.PowerMeter.Shelly.Types;
using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Utils;

namespace EnergyMonitor.Devices.PowerMeter.Shelly
{
  public class Shelly3EM : TaskBase
  {

    public Phase Phase1 { get; private set; }
    public Phase Phase2 { get; private set; }
    public Phase Phase3 { get; private set; }

    public bool RelayState { get; private set; }
    public double ActualPowerTotal { get => Phase1.Power + Phase2.Power + Phase3.Power; }

    public string PrintCurrentValues()
    {
      var builder = new StringBuilder();
      builder.AppendLine(Phase1.ToString())
        .AppendLine(Phase2.ToString())
        .AppendLine(Phase3.ToString())
        .AppendLine($"Total: {ActualPowerTotal}W");

      return builder.ToString();
    }

    public Shelly3EM() : base(1000, false)
    {
      Phase1 = new Phase();
      Phase2 = new Phase();
      Phase3 = new Phase();
    }

    private bool GetEmeterData(int index, out Emeter data)
    {
      var request = WebRequest.Create($"http://192.168.2.78/emeter/{index}");
      var response = request.GetResponse();
      data = null;

      using (var streamReader = new StreamReader(response.GetResponseStream()))
      {
        var reply = streamReader.ReadToEnd();
        data = JsonConvert.DeserializeObject<Emeter>(reply);

        return true;
      }
    }

    public void SetRelayState(bool value)
    {
      if (RelayState != value)
      {
        RelayState = value;
        var switchValue = value ? "on" : "off";
        var request = WebRequest.Create($"http://192.168.2.78/relay/0?turn={switchValue}");
        var response = request.GetResponse();

        Logging.Instance().Log(new LogMessage($"Switch Relay {switchValue}"));
      }
    }

    protected override void Run()
    {
      if (GetEmeterData(0, out var data)) Phase1.FromEmeterData(data);
      if (GetEmeterData(1, out data)) Phase2.FromEmeterData(data);
      if (GetEmeterData(2, out data)) Phase3.FromEmeterData(data);
    }
  }
}
