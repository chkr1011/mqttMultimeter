using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SimpleInjector;

namespace MQTTnet.App.Common;

sealed class ViewLocator : IDataTemplate
{
    readonly Container _container;

    public ViewLocator(Container container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
    }

    public IControl Build(object data)
    {
        var viewFullName = data.GetType().FullName!.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewFullName);

        if (viewType != null)
        {
            var control = (Control)_container.GetInstance(viewType)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock
        {
            Text = "Not Found: " + viewFullName
        };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}