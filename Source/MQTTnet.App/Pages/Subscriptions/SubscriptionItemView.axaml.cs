using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Subscriptions;

public sealed class SubscriptionItemView : UserControl
{
    public SubscriptionItemView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}