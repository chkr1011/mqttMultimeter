using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.TopicExplorer;

public partial class TopicExplorerItemView : UserControl
{
    public TopicExplorerItemView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}