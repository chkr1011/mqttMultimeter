using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed partial class TopicExplorerItemMessageView : UserControl
{
    public TopicExplorerItemMessageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}