using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MQTTnet.App.Common;
using MQTTnet.App.Controls;
using MQTTnet.App.Main;
using MQTTnet.App.Services.Mqtt;
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

        var viewLocator = new ViewLocator();
        DataTemplates.Add(viewLocator);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindowView
            {
                DataContext = _container.GetInstance<MainViewModel>()
            };

            desktop.MainWindow = _mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public static Task ShowDialog(Window window)
    {
        if (window == null)
        {
            throw new ArgumentNullException(nameof(window));
        }

        return window.ShowDialog(MainWindowView.Instance);
    }

    public static Task ShowDialog(IDialogViewModel content)
    {
        if (content == null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        var host = new Window
        {
            Title = content.Title,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInTaskbar = false,
            ShowActivated = true,
            Content = content,
            DataContext = content
        };

        return host.ShowDialog(_mainWindow);
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

        window.ShowDialog(MainWindowView.Instance);

        //ShowMessage(exception.ToString());

        //ShowDialog(new TextViewModel(exception.ToString()));
    }

    public static Task ShowMessage(string message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var host = new Window
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SizeToContent = SizeToContent.WidthAndHeight,
            ShowInTaskbar = false,
            ShowActivated = true,
            Content = message,
            DataContext = message
        };

        return host.ShowDialog(MainWindowView.Instance);
    }
}