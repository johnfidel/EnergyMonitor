using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using EnergyMonitor.Devices.PowerMeter.Shelly.Types;

namespace EnergyMonitor.Devices.PowerMeter.Shelly {
  public class Shelly3EM {
    public Shelly3EM() { }

    public bool GetEmeterData(int index, out Emeter data) {
      var request = WebRequest.Create($"http://192.168.1.37/emeter/{index}");
      var response = request.GetResponse();
      data = null;

      using (var streamReader = new StreamReader(response.GetResponseStream())) {
        var reply = streamReader.ReadToEnd();
        data = JsonConvert.DeserializeObject<Emeter>(reply);

        return true;
      }
    }
  }
}
