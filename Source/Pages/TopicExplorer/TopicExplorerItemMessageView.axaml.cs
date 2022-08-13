using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.TopicExplorer;

public partial class TopicExplorerItemMessageView : UserControl
{
    public TopicExplorerItemMessageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}