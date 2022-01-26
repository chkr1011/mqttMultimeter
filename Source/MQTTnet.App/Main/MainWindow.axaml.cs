using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Main;

public sealed class MainWindowView : Window
{
    public MainWindowView()
    {
        Instance = this;

        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif

        DataContextChanged += OnDataContextChanged;
    }

    public static MainWindowView Instance { get; private set; } = default!;

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = (MainViewModel)DataContext;

        viewModel.InflightPage.SwitchToPublishRequested += (_, _) =>
        {
            var sidebar = this.FindControl<TabControl>("Sidebar");
            sidebar.SelectedIndex = 1;
        };
    }
}