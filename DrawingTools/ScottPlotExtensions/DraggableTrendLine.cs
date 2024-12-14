using Avalonia.Input;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottables;
using System;

namespace DrawingTools.ScottPlotExtensions;

public class DraggableTrendLine
{

    private LinePlot _linePlot;
    private bool _inCreationMode = true;
    private bool _startedToDraw = false;
    private AvaPlot _plot;
    private DragableDot _anchorStart, _anchorEnd;
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    public DraggableTrendLine(AvaPlot plot, double x1, double x2, double y1, double y2)
    {
        this._plot = plot;
        this.X1 = x1;
        this.X2 = x2;
        this.Y1 = y1;
        this.Y2 = y2;

        _linePlot = _plot.Plot.Add.Line(x1, y1, x2, y2);
        _plot.PointerMoved += this._plot_PointerMoved;

        _anchorStart = new(plot, x1, y1, _linePlot.Color);
        _anchorEnd = new(plot, x2, y2, _linePlot.Color);

        _anchorEnd.OnMoved += this._anchorEnd_OnMoved;
        _anchorStart.OnMoved += this._anchorStart_OnMoved;

        _inCreationMode = true;
    }

    private void _anchorStart_OnMoved(DragableDot sender, double X, double Y)
    {
        var newCord = new Coordinates(X, Y);
        _linePlot.Start = newCord;

        _anchorStart.X = X;
        _anchorStart.Y = Y;
    }

    private void _anchorEnd_OnMoved(DragableDot sender, double X, double Y)
    {
        var newCord = new Coordinates(X, Y);
        _linePlot.End = newCord;

        _anchorEnd.X = X;
        _anchorEnd.Y = Y;
    }

    private void _plot_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_inCreationMode)
            return;

        var points = e.GetPosition(_plot);

        Pixel mousePixel = new(points.X, points.Y);
        Coordinates mouseLocation = _plot.Plot.GetCoordinates(mousePixel);
        var newCord = new Coordinates(mouseLocation.X, mouseLocation.Y);

        _linePlot.End = newCord;
        _plot.Refresh();

        if (!_startedToDraw)
        {
            _plot.PointerReleased += this._plot_PointerReleased;
            _startedToDraw = true;
        }

        _plot.UserInputProcessor.Reset();
        _plot.UserInputProcessor.Enable();
    }


    private void _plot_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _inCreationMode = false;
        _plot.PointerMoved -= this._plot_PointerMoved;

        _anchorEnd.X = _linePlot.End.X;
        _anchorEnd.Y = _linePlot.End.Y;

        _plot.PointerReleased -= _plot_PointerReleased;

    }
}