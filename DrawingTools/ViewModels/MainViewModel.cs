using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScottPlot.Avalonia;

namespace DrawingTools.ViewModels;

public partial class MainViewModel:ViewModelBase
{
    public AvaPlot AvaPlot1 { get; set; } = new AvaPlot();
    public MainViewModel()
    {
       
    }



    [RelayCommand]
    private void AddLine()
    {
    }
}
