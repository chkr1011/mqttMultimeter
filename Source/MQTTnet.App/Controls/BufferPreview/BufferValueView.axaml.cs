using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Controls;

public class BufferValueView : UserControl
{
    public BufferValueView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}