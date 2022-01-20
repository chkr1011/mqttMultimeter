using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common.ObjectDump;

public class ObjectDumpView : UserControl
{
    public ObjectDumpView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}