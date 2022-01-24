using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Inflight;

public class ReceivedApplicationMessageDetailsView : UserControl
{
    public ReceivedApplicationMessageDetailsView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}