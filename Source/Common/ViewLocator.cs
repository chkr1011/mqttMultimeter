using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace MQTTnetApp.Common;

sealed class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var viewFullName = data.GetType().FullName!.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewFullName);

        if (viewType == null)
        {
            return new TextBlock
            {
                Text = "Not Found: " + viewFullName
            };
        }

        if (Activator.CreateInstance(viewType) is not Control control)
        {
            throw new InvalidOperationException($"Unable to create view of type '{viewType.FullName}'.");
        }

        control.DataContext = data;
        return control;

    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}