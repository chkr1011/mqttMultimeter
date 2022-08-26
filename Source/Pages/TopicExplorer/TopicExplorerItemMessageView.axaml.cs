using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.TopicExplorer;

public class TopicExplorerItemMessageView : UserControl
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