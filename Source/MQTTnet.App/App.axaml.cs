using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MQTTnet.App.Common;
using MQTTnet.App.Main;
using MQTTnet.App.Services.Client;
using SimpleInjector;

namespace MQTTnet.App
{
    public sealed class App : Application
    {
        readonly Container _container;

        static Window? _mainWindow;

        public App()
        {
            _container = new Container();
            _container.Options.ResolveUnregisteredConcreteTypes = true;
            _container.RegisterSingleton<MqttClientService>();

            var viewLocator = new ViewLocator(_container);
            DataTemplates.Add(viewLocator);
        }

        public static Task ShowMessage(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

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

        public static void ShowException(Exception exception)
        {
            ShowMessage(exception.ToString());

            //ShowDialog(new TextViewModel(exception.ToString()));
        }

        public static Task ShowDialog(Window window)
        {
            if (window == null) throw new ArgumentNullException(nameof(window));

            return window.ShowDialog(MainWindowView.Instance);
        }

        public static Task ShowDialog(IDialogViewModel content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

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
                    DataContext = _container.GetInstance<MainWindowViewModel>()
                };

                desktop.MainWindow = _mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
