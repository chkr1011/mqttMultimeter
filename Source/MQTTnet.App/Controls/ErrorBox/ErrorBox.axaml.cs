using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Controls.ErrorBox;

public sealed class ErrorBox : Window
{
    public static readonly StyledProperty<string> MessageProperty = AvaloniaProperty.Register<ErrorBox, string>(nameof(Message));

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    
    public ErrorBox()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnButtonCloseClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}