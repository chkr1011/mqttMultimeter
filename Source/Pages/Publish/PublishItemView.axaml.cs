using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.Publish;

public sealed partial class PublishItemView : UserControl
{
    public PublishItemView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}