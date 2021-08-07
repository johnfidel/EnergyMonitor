using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace EnergyMonitor.Utils {
  public static class TcpClientExtension {
    public static TcpState GetState(this TcpClient tcpClient) {
      var foo = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections()
        .SingleOrDefault(x => x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint));
      return foo != null ? foo.State : TcpState.Unknown;
    }
  }
}