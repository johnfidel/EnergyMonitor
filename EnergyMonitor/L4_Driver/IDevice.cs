using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyMonitor.L4_Driver {

  delegate void DataReceivedEvent<T>(T data);

  interface IDevice { }

  interface IDevice<TBaseType> : IDevice {
    event DataReceivedEvent<TBaseType> DataReceivedEvent;
    bool Write(TBaseType data);
    bool Receive(out TBaseType data);
  }
}
