using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.Connection;

public sealed class ConnectionPageView : UserControl
{
    public ConnectionPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}