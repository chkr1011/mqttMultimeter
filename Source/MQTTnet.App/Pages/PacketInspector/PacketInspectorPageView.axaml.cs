using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MQTTnet.App.Common;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{


    public class PacketInspectorPageView : UserControl
    {
        public PacketInspectorPageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
