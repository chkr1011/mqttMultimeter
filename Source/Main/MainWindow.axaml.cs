using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnetApp.Main;

public sealed class MainWindow : Window
{
    public MainWindow()
    {
        Instance = this;

        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif

        DataContextChanged += OnDataContextChanged;
    }

    public static MainWindow Instance { get; private set; } = default!;

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