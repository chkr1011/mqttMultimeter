using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace MQTTnet.App.Common.Extensions
{
    public static class TemplatedControlExtensions
    {
        public static IControl? GetTemplateChild(this TemplatedControl? templatedControl, string name)
        {
            return templatedControl?.GetTemplateChildren()
                .FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.Ordinal));
        }
    }
}
