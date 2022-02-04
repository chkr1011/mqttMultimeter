using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace MQTTnet.App;

public class VM : ReactiveObject
{
    string _test = string.Empty;
    
    public string Test
    {
        get => _test;
        set => this.RaiseAndSetIfChanged(ref _test, value);
    }
}

public class TestWindow : Window
{
    public TestWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public ObservableCollection<VM> Items { get; } = new();

    void ClearItems(object? sender, RoutedEventArgs e)
    {
        Items.Clear();
    }

    void AddItems(object? sender, RoutedEventArgs e)
    {
        for (var i = 0; i < 10; i++)
        {
            Items.Add(new VM
            {
                Test = Guid.NewGuid().ToString()
            });
        }
    }
}