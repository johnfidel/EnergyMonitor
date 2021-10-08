using System;
using System.Collections.Generic;
using EnergyMonitor.L3_Transport.InterprocessCom.Messages;

namespace EnergyMonitor.L3_Transport.InterprocessCom {
  class IpcProtocol : IProtocol<string, IpcMessageBase> {
    public bool AssembleResponse(IpcMessageBase command, out string data) {
      data = command.ToJson();

      return true;
    }

    public bool ParseRequest(string data, out IpcMessageBase command) {
      command = null;

      var result = false;
      var receivedObject = IpcMessageBase.FromFile<IpcMessageBase>(data);

      switch ((IpcMessageBase.Type)receivedObject.MessageType) {
        case IpcMessageBase.Type.Status: {
            command = IpcMessageBase.FromJson<StatusMessage>(data);
            result = true;
            break;
          }
      }

      return result;
    }
  }
}
