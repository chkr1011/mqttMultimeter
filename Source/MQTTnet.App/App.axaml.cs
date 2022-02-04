using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MQTTnet.App.Common;
using MQTTnet.App.Controls;
using MQTTnet.App.Main;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Inflight;
using MQTTnet.App.Pages.Info;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;
using MQTTnet.App.Services.Mqtt;
using MQTTnet.App.Services.Updates;
using SimpleInjector;

namespace MQTTnet.App;

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