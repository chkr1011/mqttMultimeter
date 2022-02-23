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

        if (viewType != null)
        {
            var control = Activator.CreateInstance(viewType) as Control;
            if (control == null)
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