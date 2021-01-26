using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Subscriptions
{
    public class SubscriptionEditorView : Window
    {
        public SubscriptionEditorView()
        {
            InitializeComponent();

            GotFocus += (s, e) =>
            {
                this.FindControl<TextBox>("TextBoxTopic").Focus();
            };

            DataContextChanged += (_, __) =>
            {
                var viewModel = (SubscriptionEditorViewModel)DataContext!;

                viewModel.Completed += (___, ____) =>
                {
                    Close();
                };
            };
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
