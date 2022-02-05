using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MQTTnetApp.Common;
using MQTTnetApp.Controls;
using MQTTnetApp.Main;
using MQTTnetApp.Pages.Connection;
using MQTTnetApp.Pages.Inflight;
using MQTTnetApp.Pages.Info;
using MQTTnetApp.Pages.Publish;
using MQTTnetApp.Pages.Subscriptions;
using MQTTnetApp.Services.Mqtt;
using MQTTnetApp.Services.Updates;
using SimpleInjector;

namespace MQTTnetApp;

public sealed class App : Application
{
    static Window? _mainWindow;
    readonly Container _container;

    public App()
    {
        _container = new Container();
        _container.Options.ResolveUnregisteredConcreteTypes = true;

        _container.RegisterSingleton<MqttClientService>();

        _container.RegisterSingleton<AppUpdateService>();

        _container.RegisterSingleton<ConnectionPageViewModel>();
        _container.RegisterSingleton<PublishPageViewModel>();
        _container.RegisterSingleton<SubscriptionsPageViewModel>();
        _container.RegisterSingleton<InflightPageViewModel>();
        _container.RegisterSingleton<InfoPageViewModel>();

        var viewLocator = new ViewLocator(_container);
        DataTemplates.Add(viewLocator);

        _container.GetInstance<AppUpdateService>().EnableUpdateChecks();
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
                DataContext = _container.GetInstance<MainViewModel>()
            };
            
            desktop.MainWindow = _mainWindow;
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
}