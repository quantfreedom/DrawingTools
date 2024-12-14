using ScottPlot.Avalonia;

namespace DrawingTools.ScottPlotExtensions;

public static class DrawingExtensions
{
    public static DradableRectangle StartDrawingDragableRectangle(this AvaPlot plot, double x, double y)
    {
        var rectandle = new DradableRectangle(plot, x, x, y, y);
        return rectandle;
    }

    public static DraggableTrendLine StartDrawingDragableTrendLine(this AvaPlot plot, double x, double y)
    {
        var trendLine = new DraggableTrendLine(plot, x, x, y, y);
        return trendLine;
    }
}

