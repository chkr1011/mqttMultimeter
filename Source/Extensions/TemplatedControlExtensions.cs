using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.VisualTree;

namespace mqttMultimeter.Extensions;

public static class TemplatedControlExtensions
{
    public static StyledElement? FindVisualChild(this Visual visual, string name)
    {
        if (visual == null)
        {
            throw new ArgumentNullException(nameof(visual));
        }

        var children = visual.GetVisualChildren();

        foreach (var child in children)
        {
            if (child is StyledElement styledElement)
            {
                if (styledElement.Name == name)
                {
                    return styledElement;
                }
            }

            var c1 = child.FindVisualChild(name);
            if (c1 != null)
            {
                return c1;
            }
        }

        return null;
    }

    public static Control GetTemplateChild(this TemplatedControl templatedControl, string name)
    {
        if (templatedControl == null)
        {
            throw new ArgumentNullException(nameof(templatedControl));
        }

        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        return templatedControl.GetTemplateChildren().First(c => string.Equals(c.Name, name, StringComparison.Ordinal));
    }
}