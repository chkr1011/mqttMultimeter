namespace MQTTnet.App.Common
{
    public sealed class TextViewModel : BaseViewModel
    {
        public TextViewModel()
        {
            Text = string.Empty;
        }

        public TextViewModel(string text)
        {
            Text = text;
        }

        public string Text
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}
