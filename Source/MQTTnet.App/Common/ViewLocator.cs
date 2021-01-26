using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SimpleInjector;

namespace MQTTnet.App.Common
{
    internal sealed class ViewLocator : IDataTemplate
    {
        readonly Container _container;

        public ViewLocator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                var control = (Control)Activator.CreateInstance(type)!;
                control.DataContext = data;
                return control;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is BaseViewModel;
        }
    }
}
