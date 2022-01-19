using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Info;

public class InfoPageView : UserControl
{
    public InfoPageView()
    {
        InitializeComponent();
        
        
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}