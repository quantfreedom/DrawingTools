using Avalonia.Threading;
using Binance.Net.Clients;
using ScottPlot;
using ScottPlot.Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DrawingTools.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private AvaPlot _plot;
    private BinanceSocketClient _binanceClient = new BinanceSocketClient();
    
    public async Task FillTheChart()
    {
        var request = await _binanceClient.SpotApi.ExchangeData.GetUIKlinesAsync("BTCUSDT", Binance.Net.Enums.KlineInterval.OneHour, limit: 2000);

        if (request.Success)
        { 
            var bars = request.Data.Result.Select(x => new OHLC((double)x.OpenPrice, (double)x.HighPrice, (double)x.LowPrice, (double)x.ClosePrice, x.OpenTime, TimeSpan.FromMinutes(60))).ToArray();
             
            List<OHLC> prices = new();
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

    public ScottPlot.Plot PricePlot { get; set; }   
    public AvaPlot AvaPlot
    {
        get { return _plot; }
        set
        {
            _plot = value;
        }
    }
  
}