using Avalonia.Threading;
using Binance.Net.Clients;
using CommunityToolkit.Mvvm.Input;
using DrawingTools.ScottPlotExtensions;
using ScottPlot;
using ScottPlot.Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DrawingTools.ViewModels;

public partial class MainViewModel:ViewModelBase
{
    private AvaPlot _plot;
    private BinanceSocketClient _binanceClient = new();
    private string _drawingMode = string.Empty;

    public async Task FillTheChart()
    {
        var request = await _binanceClient.SpotApi.ExchangeData
            .GetUIKlinesAsync("BTCUSDT", Binance.Net.Enums.KlineInterval.OneHour, limit: 2000);

        if (request.Success)
        {
            var bars = request.Data.Result
                .Select(x => new OHLC(
                    (double)x.OpenPrice,
                    (double)x.HighPrice,
                    (double)x.LowPrice,
                    (double)x.ClosePrice,
                    x.OpenTime,
                    TimeSpan.FromMinutes(60)
                    )
                ).ToArray();

            List<OHLC> prices = [];
            var socket = new BinanceSocketClient();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                PricePlot.Add.Candlestick(bars);
                PricePlot.Axes.DateTimeTicksBottom();
                PricePlot.PlotControl!.Refresh();
            }, DispatcherPriority.Background);
        }
        else
        {
            Debug.WriteLine("error");
        }
    }

    public Plot PricePlot { get; set; }
    public AvaPlot AvaPlot
    {
        get { return _plot; }
        set
        {
            _plot = value;
            _plot.PointerPressed += this.Plot_PointerPressed;
        }
    }

    [RelayCommand]
    public void ClearPlot()
    {
        AvaPlot.Plot.Clear();
        AvaPlot.Refresh();
    }

    [RelayCommand]
    public void StartDrawingRect()
    {
        _drawingMode = "Rect";
    }

    [RelayCommand]
    public void StartDrawingTrendLine()
    {
        _drawingMode = "Trend Line";
    }

    private void Plot_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var plot = sender as AvaPlot;
        var points = e.GetPosition(plot);

        Pixel mousePixel = new(points.X, points.Y);
        Coordinates mouseLocation = plot!.Plot.GetCoordinates(mousePixel);


        switch (_drawingMode)
        {
            case "Rect":
                plot.StartDrawingDragableRectangle(mouseLocation.X, mouseLocation.Y);
                ResetDrawingMode();
                AvaPlot.UserInputProcessor.Disable();
                break;
            case "Trend Line":
                plot.StartDrawingDragableTrendLine(mouseLocation.X, mouseLocation.Y);
                ResetDrawingMode();
                AvaPlot.UserInputProcessor.Disable();
                break;
        }

    }

    private void ResetDrawingMode()
    {
        _drawingMode = string.Empty;
    }
}