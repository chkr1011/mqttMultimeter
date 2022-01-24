using System;
using Avalonia.Controls;
using MQTTnet.App.Common;
using MQTTnet.Client;

namespace MQTTnet.App.Pages.Info;

public sealed class InfoPageViewModel : BaseViewModel
{
    public InfoPageViewModel()
    {
        MqttNetAppVersion = typeof(App).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        MqttNetVersion = typeof(MqttClient).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        DotNetVersion = Environment.Version.ToString();
        AvaloniaVersion = typeof(Label).Assembly.GetName().Version?.ToString() ?? "<unknown>";
    }

    public string AvaloniaVersion { get; }

    public string DotNetVersion { get; }

    public string MqttNetAppVersion { get; }

    public string MqttNetVersion { get; }
}