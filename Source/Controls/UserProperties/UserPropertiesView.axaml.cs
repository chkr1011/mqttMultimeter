using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Controls;

public sealed partial class UserPropertiesView : UserControl
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