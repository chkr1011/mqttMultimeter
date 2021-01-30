using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common.ObjectDump
{
    public class ObjectDumpPropertyView : UserControl
    {
        public ObjectDumpPropertyView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
