using System.Net;
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace EnergyMonitor.Utils
{
  public static class Network
  {
    public static bool Ping(string ip)
    {
      Ping pingSender = new Ping();
      PingOptions options = new PingOptions();

      // Use the default Ttl value which is 128,
      // but change the fragmentation behavior.
      options.DontFragment = true;

      // Create a buffer of 32 bytes of data to be transmitted.
      var data = new String('a', 32);
      byte[] buffer = Encoding.ASCII.GetBytes(data);
      int timeout = 120;
      PingReply reply = pingSender.Send(IPAddress.Parse(ip), timeout, buffer, options);
      if (reply.Status == IPStatus.Success)
      {
        return true;
      }
      return false;
    }
  }
}