using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MQTTnet.App.Configuration.Model;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishPageView : UserControl
{
    public PublishPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    void OnDeleteItemClicked(object? sender, RoutedEventArgs e)
    {
        ((PublishPageViewModel) DataContext).Items.Remove((PublishItemViewModel) ((Button) sender).DataContext);
    }
}