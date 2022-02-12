using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.Log;

public class LogPageView : UserControl
{
    public LogPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}