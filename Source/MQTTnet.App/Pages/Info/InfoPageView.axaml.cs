using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Info;

public sealed class InfoPageView : UserControl
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