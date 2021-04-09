using EnergyMonitor.Devices.PowerSwitch.myStrom.Types;
using EnergyMonitor.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.Devices.PowerSwitch.myStrom {
  class MyStromSwitch : TaskBase, IPowerSwitch {
    public double Power { get; private set; }
    public bool Relay { get; private set; }
    public double Temperature { get; private set; }
    public string Ip { get; }
    public bool Connected { get => Network.Ping(Ip); }

    //{
    //  "power": 35.804927825927734,
    //  "relay": true,
    //  "temperature": 21.369983673095703
    //}
    private bool GetReport(out Report report) {
      var request = WebRequest.Create($"http://{Ip}/report");
      var response = request.GetResponse();
      report = null;

      using (var streamReader = new StreamReader(response.GetResponseStream())) {
        var reply = streamReader.ReadToEnd();
        report = JsonConvert.DeserializeObject<Report>(reply);

        return true;
      }
    }

    protected override void Run() {
      if (GetReport(out var report)) {
        Power = report.power;
        Relay = report.relay;
        Temperature = report.temperature;
      }
    }

    public MyStromSwitch(string deviceIp) : base(100, false) {
      Ip = deviceIp;
    }
  }
}
