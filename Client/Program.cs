using EnergyMonitor.L4_Driver.Socket;
using System;

namespace Client {
  class Program {
    static void Main(string[] args) {
      var client = new TcpSocketClient("127.0.0.1", 8888);
      client.DataReceivedEvent += Client_DataReceivedEvent;

      Console.Read();
    }

    private static void Client_DataReceivedEvent(string data) {
      Console.Write(data);
    }
  }
}
