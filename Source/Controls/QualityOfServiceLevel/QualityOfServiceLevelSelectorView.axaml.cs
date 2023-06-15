using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Controls;

public sealed partial class QualityOfServiceLevelSelectorView : UserControl
{
    public QualityOfServiceLevelSelectorView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}