using Newtonsoft.Json;
using EnergyMonitor.Types;
using EnergyMonitor.BusinessLogic;

namespace EnergyMonitor.L3_Transport.InterprocessCom.Messages {
  class StatusMessage : IpcMessageBase { 
    public State Status { get; set; }   
    
    public StatusMessage() {
      MessageType = Type.Status;
    }
  }
}
