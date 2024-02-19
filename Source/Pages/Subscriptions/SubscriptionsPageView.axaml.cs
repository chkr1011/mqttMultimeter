using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.Subscriptions;

public sealed partial class SubscriptionsPageView : UserControl
{
    public SubscriptionsPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}