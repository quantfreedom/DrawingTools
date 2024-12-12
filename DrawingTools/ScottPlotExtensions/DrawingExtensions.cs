using ScottPlot.Avalonia;

namespace DrawingTools.ScottPlotExtensions;

public static class DrawingExtensions
{
    public static DradableRectangle StartDrawingDragableRectangle(this AvaPlot plot, double x, double y)
    {
        var rectandle = new DradableRectangle(plot, x, x, y, y);
        return rectandle;
    }
}

