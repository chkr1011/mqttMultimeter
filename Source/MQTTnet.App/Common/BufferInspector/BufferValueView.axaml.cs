using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common.BufferInspector
{
    public class BufferValueView : UserControl
    {
        public BufferValueView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
