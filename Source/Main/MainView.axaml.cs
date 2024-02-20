using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Main;

public sealed partial class MainView : UserControl
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

        foreach (TabItem? tabItem in sidebar.Items)
        {
            if (tabItem == null)
            {
                continue;
            }

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

    void OnUpdateAvailableNotificationPressed(object? _, PointerPressedEventArgs __)
    {
        ((MainViewModel)DataContext!).InfoPage.OpenReleasesUrl();
    }
}