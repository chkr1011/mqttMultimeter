using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Threading;
using mqttMultimeter.Common;
using mqttMultimeter.Services.Updates;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Info;

public sealed class InfoPageViewModel : BasePageViewModel
{
    readonly AppUpdateService _appUpdateService;

    bool _isUpdateAvailable;
    string _latestAppVersion = string.Empty;

    public InfoPageViewModel(AppUpdateService appUpdateService)
    {
        _appUpdateService = appUpdateService ?? throw new ArgumentNullException(nameof(appUpdateService));

        CurrentAppVersion = appUpdateService.CurrentVersion.ToString();
        MqttNetVersion = typeof(MqttClient).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        DotNetVersion = Environment.Version.ToString();
        AvaloniaVersion = typeof(Label).Assembly.GetName().Version?.ToString() ?? "<unknown>";

        DispatcherTimer.Run(CheckForUpdates, TimeSpan.FromSeconds(1));
    }

    public string AvaloniaVersion { get; }

    public string CurrentAppVersion { get; }

    public string DotNetVersion { get; }

    public bool IsUpdateAvailable
    {
        get => _isUpdateAvailable;
        set => this.RaiseAndSetIfChanged(ref _isUpdateAvailable, value);
    }

    public string LatestAppVersion
    {
        get => _latestAppVersion;
        set => this.RaiseAndSetIfChanged(ref _latestAppVersion, value);
    }

    public string MqttNetVersion { get; }

    public void OpenReleasesUrl()
    {
        Launch("https://github.com/chkr1011/mqttMultimeter/releases");
    }

    bool CheckForUpdates()
    {
        LatestAppVersion = _appUpdateService.LatestVersion?.ToString() ?? string.Empty;
        IsUpdateAvailable = _appUpdateService.IsUpdateAvailable;

        // Keep timer running.
        return true;
    }

    static void Launch(string fileName)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        });
    }
}