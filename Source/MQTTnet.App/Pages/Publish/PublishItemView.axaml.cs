using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishItemView : UserControl
{
    public PublishItemView()
    {
        AvaloniaXamlLoader.Load(this);
    }
}