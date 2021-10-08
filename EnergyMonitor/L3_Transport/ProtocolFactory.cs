using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.L3_Transport {
  static class ProtocolFactory {
    public static IProtocol CreateProtocol(Protocols type) {
      switch (type) {
        case Protocols.IpcProtocol: {
            return new L3_Transport.InterprocessCom.IpcProtocol();
          }
      }

      return null;
    }
  }
}
