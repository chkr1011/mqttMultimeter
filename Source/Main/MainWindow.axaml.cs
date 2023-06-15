using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Main;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Instance = this;

        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    public static MainWindow Instance { get; private set; } = default!;

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}