using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common.BufferInspector
{
    public class BufferInspectorView : UserControl
    {
        public BufferInspectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
