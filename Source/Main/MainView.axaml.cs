using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Main;

public sealed class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnActivatePageRequested(object? sender, EventArgs e)
    {
        var sidebar = this.FindControl<TabControl>("Sidebar");

        if (sidebar == null)
        {
            return;
        }
        
        foreach (TabItem tabItem in sidebar.Items)
        {
            if (ReferenceEquals(sender, tabItem.Content))
            {
                sidebar.SelectedItem = tabItem;
            }
        }
    }

    void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = (MainViewModel?)DataContext;

        if (viewModel == null)
        {
            return;
        }

        viewModel.ActivatePageRequested += OnActivatePageRequested;
    }
}