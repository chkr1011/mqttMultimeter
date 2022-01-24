using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Inflight;

public class InflightPageView : UserControl
{
    public InflightPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}