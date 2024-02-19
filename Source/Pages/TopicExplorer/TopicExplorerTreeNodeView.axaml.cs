using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed partial class TopicExplorerTreeNodeView : UserControl
{
    public TopicExplorerTreeNodeView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = (TopicExplorerTreeNodeViewModel)DataContext!;
        viewModel.MessagesChanged += OnMessagesChanged;
    }

    void OnMessagesChanged(object? sender, EventArgs eventArgs)
    {
        Control? control = this;
        while (control is not TreeViewItem)
        {
            control = control.GetVisualParent<Control>();
            if (control == null)
            {
                break;
            }
        }

        if (control != null)
        {
            var viewModel = (TopicExplorerTreeNodeViewModel)control.DataContext!;
            if (!viewModel.OwnerPage.HighlightChanges)
            {
                return;
            }

            var treeViewItem = (TreeViewItem)control;

            treeViewItem.Classes.Add("highlight");

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));

                treeViewItem.Classes.Remove("highlight");
            });
        }
    }
}