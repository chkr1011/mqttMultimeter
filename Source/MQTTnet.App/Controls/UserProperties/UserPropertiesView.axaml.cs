using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Controls;

public sealed class UserPropertiesView : UserControl
{
    public UserPropertiesView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}