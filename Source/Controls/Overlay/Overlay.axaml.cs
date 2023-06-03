using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace mqttMultimeter.Controls;

public sealed class Overlay : ContentControl
{
    public static readonly StyledProperty<bool> IsOverlayVisibleProperty = AvaloniaProperty.Register<Overlay, bool>(nameof(IsOverlayVisible));

    public static readonly StyledProperty<object?> OverlayContentProperty = AvaloniaProperty.Register<Overlay, object?>(nameof(OverlayContent));

    public bool IsOverlayVisible
    {
        get => GetValue(IsOverlayVisibleProperty);
        set => SetValue(IsOverlayVisibleProperty, value);
    }

    public object? OverlayContent
    {
        get => GetValue(OverlayContentProperty);
        set => SetValue(OverlayContentProperty, value);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        // if (IsOverlayVisible)
        // {
        //     e.Handled = true;
        //     return;
        // }

        base.OnKeyDown(e);
    }
}