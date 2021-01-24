namespace MQTTnet.App.Common
{
    public abstract class BasePageViewModel : BaseViewModel
    {
        public object Header
        {
            get => GetValue<object>();
            set => SetValue(value);
        }
    }
}