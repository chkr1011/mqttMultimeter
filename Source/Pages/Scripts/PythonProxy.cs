// ReSharper disable InconsistentNaming

using IronPython.Runtime;

namespace MQTTnetApp.Pages.Scripts;

public sealed class PythonProxy
{
    public PythonDictionary publish(string topic, object payload)
    {
        return new PythonDictionary();
    }
}