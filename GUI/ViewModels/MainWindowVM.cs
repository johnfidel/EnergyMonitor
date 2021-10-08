using System;
using System.Collections.Generic;
using System.Text;
using EnergyMonitor.BusinessLogic;
using EnergyMonitor.Utils;

namespace GUI.ViewModels {
  public class MainWindowVM : ViewModelBase {
    public string Greeting => "Welcome to Avalonia!";

    public double[] DataX { get; set; } = new double[] { 1, 2, 3, 4 };
    public double[] DataY { get; set; } = new double[] { 1, 2, 3, 4 };

    public MainWindowVM() {
      var state = Serializable.FromJson<State>(System.IO.File.ReadAllText(State.FILENAME));
    }
  }
}
