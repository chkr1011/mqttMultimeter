using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Pages.TopicExplorer;

public partial class TopicExplorerItemView : UserControl
{
    public TopicExplorerItemView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}