using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Controls;

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