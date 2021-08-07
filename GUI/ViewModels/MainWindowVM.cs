using System;
using System.Collections.Generic;
using System.Text;

namespace GUI.ViewModels {
  public class MainWindowVM : ViewModelBase {
    public string Greeting => "Welcome to Avalonia!";

    public double[] DataX { get; set; } = new double[] { 1, 2, 3, 4 };
    public double[] DataY { get; set; } = new double[] { 1, 2, 3, 4 };
  }
}
