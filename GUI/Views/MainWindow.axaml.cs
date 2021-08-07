using Avalonia;
using ScottPlot.Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;

namespace GUI.Views {
  public partial class MainWindow : Window {
    private MainWindowVM ViewModel { get; set; }

    public MainWindow() {}
    
    public MainWindow(MainWindowVM viewModel) {
      InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
      DataContext = ViewModel = viewModel;

      AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1");
      
      avaPlot1.Plot.AddScatter(ViewModel.DataX, ViewModel.DataY);
    }

    private void InitializeComponent() {
      AvaloniaXamlLoader.Load(this);
    }
  }
}