using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Common.QualityOfServiceLevel
{
    public class QualityOfServiceLevelSelectorView : UserControl
    {
        public QualityOfServiceLevelSelectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
