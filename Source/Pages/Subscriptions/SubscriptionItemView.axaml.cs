using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.Subscriptions;

public sealed partial class SubscriptionItemView : UserControl
{
    public SubscriptionItemView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}