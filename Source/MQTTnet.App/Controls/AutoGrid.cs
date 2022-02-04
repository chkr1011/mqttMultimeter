using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace MQTTnet.App.Controls;

public class AutoGrid : Grid
{
    public static readonly AttachedProperty<bool>
        IsNextRowProperty = AvaloniaProperty.RegisterAttached<AutoGrid, Interactive, bool>("IsNextRow", false, false, BindingMode.Default);

    public static bool GetIsNextRow(AvaloniaObject element)
    {
        return element.GetValue(IsNextRowProperty);
    }

    public static void SetIsNextRow(AvaloniaObject element, bool value)
    {
        element.SetValue(IsNextRowProperty, value);
    }

    protected override void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        base.ChildrenChanged(sender, e);

        var currentRow = 0;

        foreach (var child in Children)
        {
            if (child is Control control)
            {
                if (GetIsNextRow(control))
                {
                    currentRow++;
                }

                SetRow(control, currentRow);
            }
        }

        if (RowDefinitions.Count - 1 != currentRow)
        {
            RowDefinitions.Clear();
            for (var i = 0; i <= currentRow; i++)
            {
                RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }
        }
    }
}