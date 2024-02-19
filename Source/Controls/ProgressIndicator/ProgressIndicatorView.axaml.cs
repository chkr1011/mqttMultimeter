using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Controls;

public sealed partial class ProgressIndicatorView : UserControl
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