using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MQTTnet.App.Common;

namespace MQTTnet.App;

sealed class ViewLocator : IDataTemplate
{
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

        return new TextBlock
        {
            Text = "Not Found: " + name
        };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}