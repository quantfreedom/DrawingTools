using Avalonia.Controls;
using DrawingTools.ViewModels;
using ScottPlot.Avalonia;

namespace DrawingTools.Views
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += this.MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var plot = this.Find<AvaPlot>("AvaPlot1");
           
            (DataContext as MainViewModel)!.PricePlot = plot!.Plot;
            (DataContext as MainViewModel)!.AvaPlot = plot!;

            await (this.DataContext as MainViewModel)!.FillTheChart();             
        } 
     
    }
}