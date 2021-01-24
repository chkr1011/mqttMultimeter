using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common
{
    public class TextView : UserControl
    {
        public TextView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
