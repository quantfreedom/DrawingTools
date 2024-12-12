using Avalonia.Controls;
using Binance.Net.Objects.Models.Spot.SubAccountData;
using DrawingTools.ScottPlotExtensions;
using DrawingTools.ViewModels;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using System.Threading.Tasks;

namespace DrawingTools.Views
{
    public partial class MainWindow : Window
    {
        private bool _drawingMode = false;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += this.MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var plot = this.Find<AvaPlot>("AvaPlot1");
           
            var btn = this.Find<Button>("btn");
            (DataContext as MainViewModel)!.PricePlot = plot!.Plot;
            (DataContext as MainViewModel)!.AvaPlot = plot!;

            await (this.DataContext as MainViewModel)!.FillTheChart();

            plot.PointerPressed += this.Plot_PointerPressed;

            btn.Click += this.Btn_Click;
        }

        private void Btn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _drawingMode = true;
        }

        private void Plot_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            if (!_drawingMode)
                return;

            var plot = sender as AvaPlot;
            var points = e.GetPosition(plot);

            Pixel mousePixel = new(points.X, points.Y);
            Coordinates mouseLocation = plot.Plot.GetCoordinates(mousePixel);

            plot!.StartDrawingDragableRectangle(mouseLocation.X, mouseLocation.Y);
            _drawingMode = false;
        }
    }
}