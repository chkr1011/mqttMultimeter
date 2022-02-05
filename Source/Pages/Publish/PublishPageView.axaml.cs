using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.Publish;

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

    // private void OnDeleteItemClicked(object? sender, RoutedEventArgs e)
    // {
    //     ((PublishPageViewModel)DataContext).Items.Remove((PublishItemViewModel)((Button)sender).DataContext);
    // }
}