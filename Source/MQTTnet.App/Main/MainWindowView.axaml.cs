using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Main
{
    public class MainWindowView : Window
    {
        public static MainWindowView Instance { get; private set; } = default!;

        public MainWindowView()
        {
            Instance = this;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
