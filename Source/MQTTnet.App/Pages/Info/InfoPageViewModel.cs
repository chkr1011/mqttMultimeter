using System;
using MQTTnet.App.Common;
using MQTTnet.Client;
using ReactiveUI;

namespace MQTTnet.App.Pages.Info;

public sealed class InfoPageViewModel : BasePageViewModel
{
    public InfoPageViewModel()
    {
        Header = "Info";

        MqttNetAppVersion = typeof(App).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        MqttNetVersion = typeof(MqttClient).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        DotNetVersion = Environment.Version.ToString();
    }
    
    public string MqttNetAppVersion { get; }
    
    public string MqttNetVersion { get; }
    
    public string DotNetVersion { get; }
}