using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.TopicExplorer;

public partial class TopicExplorerPageView : UserControl
{
    public TopicExplorerPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}