using Avalonia;
using Avalonia.Controls;

namespace MQTTnet.App.Controls;

public sealed class AppTabControl : TabControl
{
    public static readonly StyledProperty<object?> CustomHeaderContentProperty = AvaloniaProperty.Register<TabControl, object?>(nameof(TabStripPlacement));

    public object? CustomHeaderContent
    {
        get => GetValue(CustomHeaderContentProperty);
        set => SetValue(CustomHeaderContentProperty, value);
    }
}