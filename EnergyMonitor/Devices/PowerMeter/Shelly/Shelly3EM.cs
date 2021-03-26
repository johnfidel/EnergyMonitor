using System.Diagnostics.Tracing;
using System.Text;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using EnergyMonitor.Devices.PowerMeter.Shelly.Types;
using EnergyMonitor.Devices.PowerMeter.Types;
using EnergyMonitor.Utils;
using EnergyMonitor.Types;

namespace EnergyMonitor.Devices.PowerMeter.Shelly {
  public class Shelly3EM : TaskBase, IPowermeter {


    public bool Connected { get => Network.Ping(Ip); }
    public string Ip { get; private set; }
    public Phase Phase1 { get; private set; }
    public Phase Phase2 { get; private set; }
    public Phase Phase3 { get; private set; }

    public OutputState RelayState { get; private set; }
    public double ActualPowerTotal { get => Phase1.Power + Phase2.Power + Phase3.Power; }

    public string PrintCurrentValues() {
      var builder = new StringBuilder();
      builder.AppendLine(Phase1.ToString())
        .AppendLine(Phase2.ToString())
        .AppendLine(Phase3.ToString())
        .AppendLine($"Total: {ActualPowerTotal}W");

      return builder.ToString();
    }

    public Shelly3EM(string deviceIp) : base(1000, false) {
      Ip = deviceIp;
      Phase1 = new Phase();
      Phase2 = new Phase();
      Phase3 = new Phase();
    }

    private bool GetEmeterData(int index, out Emeter data) {
      var request = WebRequest.Create($"http://{Ip}/emeter/{index}");
      var response = request.GetResponse();
      data = null;

      using (var streamReader = new StreamReader(response.GetResponseStream())) {
        var reply = streamReader.ReadToEnd();
        data = JsonConvert.DeserializeObject<Emeter>(reply);

        return true;
      }
    }

    public void SetRelayState(OutputState value) {
      if (RelayState != value || RelayState == OutputState.Unknown) {
        RelayState = value;
        var switchValue = value == OutputState.On ? "on" : "off";
        var request = WebRequest.Create($"http://{Ip}/relay/0?turn={switchValue}");
        var response = request.GetResponse();

        Logging.Instance().Log(new LogMessage($"Switch Relay {switchValue}"));
      }
    }

    protected override void Run() {
      if (GetEmeterData(0, out var data)) Phase1.FromEmeterData(data);
      if (GetEmeterData(1, out data)) Phase2.FromEmeterData(data);
      if (GetEmeterData(2, out data)) Phase3.FromEmeterData(data);
    }
  }
}
