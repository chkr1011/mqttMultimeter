using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Publish;

public class PublishPageView : UserControl
{
    public PublishPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnCloseItemClicked(object? sender, RoutedEventArgs e)
    {
        ((PublishPageViewModel)DataContext).Items.Remove((PublishItemViewModel)((Button)sender).DataContext);
    }
}