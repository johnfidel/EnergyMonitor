using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EnergyMonitor.BusinessLogic;
using EnergyMonitor.L4_Driver.Socket;
using EnergyMonitor.Utils;
using ReactiveUI.Fody.Helpers;

namespace GUI.ViewModels {
  public class MainWindowVM : ViewModelBase {

    private TcpSocketClient Client { get; set; }

    [Reactive]
    public double SolarPower { get; set; }
    [Reactive]
    public double PhaseA { get; set; }
    [Reactive]
    public double PhaseB { get; set; }
    [Reactive]
    public double PhaseC { get; set; }
    [Reactive]
    public double TotalPower { get; set; }
    [Reactive]
    public double TotalAveragePower { get; set; }

    public MainWindowVM() {
      Client = new TcpSocketClient("127.0.0.1", 8888);
      Client.DataReceivedEvent += Client_DataReceivedEvent;
    }

    private void Client_DataReceivedEvent(string data) {
      var state = Serializable.FromJson<State>(data);
      SolarPower = state.SolarPower;
      PhaseA = state.CurrentPhaseAPower;
      PhaseB = state.CurrentPhaseBPower;
      PhaseC = state.CurrentPhaseCPower;
      TotalPower = state.CurrentPower;
      TotalAveragePower = state.ActualAveragePower;
    }
  }
}
