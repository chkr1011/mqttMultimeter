using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Microsoft.Extensions.DependencyInjection;

namespace MQTTnetApp.Common;

sealed class ViewLocator : IDataTemplate
{
    readonly IServiceProvider _serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IControl Build(object data)
    {
        var viewFullName = data.GetType().FullName!.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewFullName);

        if (viewType != null)
        {
            if (_serviceProvider.GetRequiredService(viewType) is not Control control)
            {
                throw new InvalidOperationException($"Unable to create view of type '{viewType.FullName}'.");
            }

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