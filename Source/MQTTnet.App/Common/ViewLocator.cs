using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace MQTTnet.App.Common;

sealed class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var viewFullName = data.GetType().FullName!.Replace("ViewModel", "View");
        var viewType = Type.GetType(viewFullName);

        if (viewType != null)
        {
            var control = (Control)Activator.CreateInstance(viewType)!;
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