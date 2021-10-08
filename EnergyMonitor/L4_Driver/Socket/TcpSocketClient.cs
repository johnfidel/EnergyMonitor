using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using EnergyMonitor.Utils;

namespace EnergyMonitor.L4_Driver.Socket {

  class TcpSocketClient : TaskBase, IDevice<string>, IDisposable {
    public event DataReceivedEvent<string> DataReceivedEvent;

    protected TcpClient Client { get; set; }

    public TcpSocketClient(string ip, int port) : base(100, true) {
      Client = new TcpClient(ip, port);
      Start();
    }

    protected override void Run() {
      if (Receive(out var data)) {
        DataReceivedEvent?.Invoke(data);
      }
    }

    public bool Write(string data) {
      var stream = Client?.GetStream();
      stream?.Write(Encoding.UTF8.GetBytes(data));

      return true;
    }

    public bool Receive(out string data) {
      data = null;
      var stream = Client.GetStream();
      if (stream.DataAvailable) {
        byte[] buffer = new byte[4096];
        var size = stream.Read(buffer);
        Array.Resize(ref buffer, size);
        data = Encoding.UTF8.GetString(buffer);
        DataReceivedEvent?.Invoke(data);

        return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing) {
      base.Dispose(disposing);

      Client.Close();
      Client.Dispose();
    }
  }
}
