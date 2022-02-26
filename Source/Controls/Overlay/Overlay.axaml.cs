using Avalonia;
using Avalonia.Controls;

namespace MQTTnetApp.Controls;

public sealed class Overlay : ContentControl
{
    public static readonly StyledProperty<bool> IsOverlayVisibleProperty = AvaloniaProperty.Register<Overlay, bool>(nameof(IsOverlayVisible));

    public bool IsOverlayVisible
    {
        get => GetValue(IsOverlayVisibleProperty);
        set => SetValue(IsOverlayVisibleProperty, value);
    }
}