namespace MQTTnet.App.Common
{
    public sealed class BufferValueViewModel : BaseViewModel
    {
        public BufferValueViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}