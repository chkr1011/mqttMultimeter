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

    void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = (MainViewModel?)DataContext;

        if (viewModel == null)
        {
            return;
        }

        viewModel.InflightPage.SwitchToPublishRequested += (_, _) =>
        {
            var sidebar = this.FindControl<TabControl>("Sidebar");
            sidebar.SelectedIndex = 1;
        };
    }
}