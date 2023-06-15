using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.Publish;

public sealed partial class PublishPageView : UserControl
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