using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using mqttMultimeter.Main;
using mqttMultimeter.Pages.Connection;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Pages.Inflight.Export;
using mqttMultimeter.Pages.Info;
using mqttMultimeter.Pages.Log;
using mqttMultimeter.Pages.PacketInspector;
using mqttMultimeter.Pages.Publish;
using mqttMultimeter.Pages.Subscriptions;
using mqttMultimeter.Pages.TopicExplorer;
using mqttMultimeter.Services.Data;
using mqttMultimeter.Services.Mqtt;
using mqttMultimeter.Services.State;
using mqttMultimeter.Services.Updates;

namespace mqttMultimeter;

public sealed class App : Application
{
    static MainViewModel? _mainViewModel;

    readonly StateService _stateService;

    public App()
    {
        var serviceProvider = new ServiceCollection()
            // Services
            .AddSingleton<MqttClientService>()
            .AddSingleton<AppUpdateService>()
            .AddSingleton<JsonSerializerService>()
            .AddSingleton<StateService>()
            .AddSingleton<InflightPageItemExportService>()
            // Pages
            .AddSingleton<ConnectionPageViewModel>()
            .AddSingleton<PublishPageViewModel>()
            .AddSingleton<SubscriptionsPageViewModel>()
            .AddSingleton<PacketInspectorPageViewModel>()
            .AddSingleton<TopicExplorerPageViewModel>()
            .AddSingleton<InflightPageViewModel>()
            .AddSingleton<LogPageViewModel>()
            .AddSingleton<InfoPageViewModel>()
            .AddSingleton<MainViewModel>()
            .BuildServiceProvider();

        var viewLocator = new ViewLocator();
        DataTemplates.Add(viewLocator);

        serviceProvider.GetRequiredService<AppUpdateService>().EnableUpdateChecks();
        _stateService = serviceProvider.GetRequiredService<StateService>();
        _mainViewModel = serviceProvider.GetService<MainViewModel>();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Current!.RequestedThemeVariant = ThemeVariant.Dark;
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };

            mainWindow.Closing += (_, __) =>
            {
                _stateService.Write().GetAwaiter().GetResult();
            };

            desktop.MainWindow = mainWindow;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static void ShowException(Exception exception)
    {
        var viewModel = new ErrorBoxViewModel
        {
            Message = exception.Message,
            Exception = exception.ToString()
        };

        var errorBox = new ErrorBox
        {
            DataContext = viewModel
        };

        errorBox.Closed += (_, __) =>
        {
            // Consider using a Stack so that multiple contents like windows etc. can be stacked.
            _mainViewModel!.OverlayContent = null;
        };

        _mainViewModel!.OverlayContent = errorBox;
    }
}