using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Controls;

public class ProgressIndicatorView : UserControl
{
    public ProgressIndicatorView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}