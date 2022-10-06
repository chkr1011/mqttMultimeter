using Avalonia.Data.Converters;

namespace MQTTnetApp.Converters;

public static class ObjectConverter
{
    public static IValueConverter IsNotNull { get; } = new FuncValueConverter<object, bool>(p => p != null);
    public static IValueConverter IsNull { get; } = new FuncValueConverter<object, bool>(p => p == null);
}