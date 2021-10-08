using EnergyMonitor.Utils;

namespace EnergyMonitor.L3_Transport.InterprocessCom.Messages {

  class IpcMessageBase : Serializable {
    public enum Type {
      Status
    }

    public Type MessageType { get; set; }
  }
}
