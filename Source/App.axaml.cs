using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MQTTnetApp.Common;
using MQTTnetApp.Controls;
using MQTTnetApp.Main;
using MQTTnetApp.Pages.Connection;
using MQTTnetApp.Pages.Inflight;
using MQTTnetApp.Pages.Info;
using MQTTnetApp.Pages.Log;
using MQTTnetApp.Pages.PacketInspector;
using MQTTnetApp.Pages.Publish;
using MQTTnetApp.Pages.Subscriptions;
using MQTTnetApp.Services.Data;
using MQTTnetApp.Services.Mqtt;
using MQTTnetApp.Services.State;
using MQTTnetApp.Services.Updates;

namespace MQTTnetApp;

public sealed class App : Application
{
    static Window? _mainWindow;
    readonly MainViewModel? _mainViewModel;

    readonly StateService _stateService;

    public App()
    {
        var serviceProvider = new ServiceCollection().AddLogging()
            .AddSingleton<MqttClientService>()
            .AddSingleton<AppUpdateService>()
            .AddSingleton<JsonSerializerService>()
            .AddSingleton<StateService>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<ConnectionPageViewModel>()
            .AddSingleton<PublishPageViewModel>()
            .AddSingleton<SubscriptionsPageViewModel>()
            .AddSingleton<PacketInspectorPageViewModel>()
            .AddSingleton<InflightPageViewModel>()
            .AddSingleton<LogPageViewModel>()
            .AddSingleton<InfoPageViewModel>()
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
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainWindow.Closing += OnMainWindowClosing;

            desktop.MainWindow = _mainWindow;
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

        var window = new ErrorBox
        {
            DataContext = viewModel,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        window.ShowDialog(MainWindow.Instance);
    }

    void OnMainWindowClosing(object? sender, CancelEventArgs e)
    {
        _stateService.Write().GetAwaiter().GetResult();
    }
}