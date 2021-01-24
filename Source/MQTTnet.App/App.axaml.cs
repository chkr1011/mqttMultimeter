using Avalonia;
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

        public App()
        {
            _container = new Container();
            _container.Options.ResolveUnregisteredConcreteTypes = true;
            _container.RegisterSingleton<MqttClientService>();

            var viewLocator = new ViewLocator(_container);

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
                desktop.MainWindow = new MainWindowView
                {
                    DataContext = _container.GetInstance<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
