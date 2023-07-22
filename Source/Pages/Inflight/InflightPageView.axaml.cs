using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace mqttMultimeter.Pages.Inflight;

public sealed partial class InflightPageView : UserControl
{
    const string ExportDialogTitle = "Export Inflight items";

    static readonly List<FilePickerFileType> ExportDialogFilter = new()
    {
        new FilePickerFileType("MQTT Inflight item exports")
        {
            Patterns = new[]
            {
                "*.mqtt_inflight"
            }
        }
    };

    public InflightPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    void OnExportItems(object? _, RoutedEventArgs __)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                var filePickerOptions = new FilePickerSaveOptions
                {
                    Title = ExportDialogTitle,
                    FileTypeChoices = ExportDialogFilter
                };

                var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(filePickerOptions);
                if (file == null)
                {
                    return;
                }

                await ((InflightPageViewModel)DataContext!).ExportItems(file.Path.LocalPath);
            }
            catch (FileNotFoundException)
            {
                // Ignore this case!
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        });
    }

    void OnImportItems(object? _, RoutedEventArgs __)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                var filePickerOptions = new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    Title = ExportDialogTitle,
                    FileTypeFilter = ExportDialogFilter
                };

                var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(filePickerOptions);
                if (files.Count == 0)
                {
                    return;
                }

                await ((InflightPageViewModel)DataContext!).ImportItems(files[0].Path.LocalPath);
            }
            catch (FileNotFoundException)
            {
                // Ignore this case!
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        });
    }
}