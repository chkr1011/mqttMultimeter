using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Main;

public class MainWindowView : Window
{
    public MainWindowView()
    {
        Instance = this;

        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    public static MainWindowView Instance { get; private set; } = default!;

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}