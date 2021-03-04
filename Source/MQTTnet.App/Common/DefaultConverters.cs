namespace MQTTnet.App.Common
{
    public static class DefaultConverters
    {
        public static BooleanInverter BooleanInverter { get; } = new BooleanInverter();

        public static RadioButtonValueConverter RadioButtonValueConverter { get; } = new RadioButtonValueConverter();
    }
}
