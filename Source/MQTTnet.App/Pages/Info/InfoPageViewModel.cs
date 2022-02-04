using System;
using Avalonia.Controls;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Updates;
using MQTTnet.Client;
using ReactiveUI;

namespace MQTTnet.App.Pages.Info;

public sealed class InfoPageViewModel : BaseViewModel
{
    readonly AppUpdateService _appUpdateService;

    bool _isUpdateAvailable;
    string _latestAppVersion = string.Empty;

    public InfoPageViewModel(AppUpdateService appUpdateService)
    {
        _appUpdateService = appUpdateService ?? throw new ArgumentNullException(nameof(appUpdateService));
        
        CurrentAppVersion = appUpdateService.CurrentVersion.ToString();
        AppVersion = typeof(App).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        MqttNetVersion = typeof(MqttClient).Assembly.GetName().Version?.ToString() ?? "<unknown>";
        DotNetVersion = Environment.Version.ToString();
        AvaloniaVersion = typeof(Label).Assembly.GetName().Version?.ToString() ?? "<unknown>";

        DispatcherTimer.Run(CheckForUpdates, TimeSpan.FromSeconds(1));
    }

    public string AppVersion { get; }

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

    bool CheckForUpdates()
    {
        LatestAppVersion = _appUpdateService.LatestVersion?.ToString() ?? string.Empty;
        IsUpdateAvailable = _appUpdateService.IsUpdateAvailable;

        // Keep timer running.
        return true;
    }
}