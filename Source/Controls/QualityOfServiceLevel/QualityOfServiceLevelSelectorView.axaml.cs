using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Controls;

public sealed class QualityOfServiceLevelSelectorView : UserControl
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