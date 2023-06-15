using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Controls;

public sealed partial class ErrorBox : UserControl
{
    public static readonly StyledProperty<string> MessageProperty = AvaloniaProperty.Register<ErrorBox, string>(nameof(Message));

    public ErrorBox()
    {
        InitializeComponent();
    }

    public event EventHandler? Closed;

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnButtonCloseClicked(object? sender, RoutedEventArgs e)
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }

    void OnButtonCopyClicked(object? sender, RoutedEventArgs e)
    {
        //_ = Application.Current?.Clipboard?.SetTextAsync(Message);
    }
}