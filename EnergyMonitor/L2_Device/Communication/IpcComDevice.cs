using System.Collections.Generic;
using System.Net.Sockets;
using EnergyMonitor.L3_Transport;
using EnergyMonitor.L3_Transport.InterprocessCom.Messages;
using EnergyMonitor.L4_Driver;
using EnergyMonitor.L4_Driver.Socket;
using EnergyMonitor.Utils;

namespace EnergyMonitor.L2_Device.Communication {

  delegate void CommandReceived<T>(object sender, T message);

  class IpcComDevice {
    protected IDevice<string> ServerSocket { get; set; }
    protected IDevice<string> ClientSocket { get; set; }
    protected IProtocol Protocol { get; set; }

    public event CommandReceived<IpcMessageBase> MessageReceivedEvent;

    public IpcComDevice(bool server) {
      if (server) {
        ServerSocket = new TcpSocketServer(55000);
        ServerSocket.DataReceivedEvent += DataReceivedEvent;
      }
      else {
        ClientSocket = new TcpSocketClient("localhost", 55000);
        ClientSocket.DataReceivedEvent += DataReceivedEvent;
      }

      Protocol = ProtocolFactory.CreateProtocol(Protocols.IpcProtocol);
    }

    private void DataReceivedEvent(string data) {
      var protocol = Protocol as IProtocol<string, IpcMessageBase>;
      if (protocol.ParseRequest(data, out var message)) {
        MessageReceivedEvent?.Invoke(this, message);
      }
    }

    public bool Send(IpcMessageBase msg) {
      var protocol = Protocol as IProtocol<string, IpcMessageBase>;
      if (protocol.AssembleResponse(msg, out var data)) {
        return ((ServerSocket?.Write(data) ?? false) || (ClientSocket?.Write(data) ?? false));
      }

      return false;
    }
  }
}
