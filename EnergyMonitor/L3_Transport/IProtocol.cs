using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.L3_Transport {
  enum Protocols {
    IpcProtocol
  }

  interface IProtocol { }

  interface IProtocol<TBaseType, TCommand> : IProtocol {    
    bool AssembleResponse(TCommand command, out TBaseType data);
    bool ParseRequest(TBaseType data, out TCommand command);
  }
}
