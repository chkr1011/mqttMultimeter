using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using mqttMultimeter.Extensions;

namespace mqttMultimeter.Controls;

public sealed class CertificatePicker : TemplatedControl
{
    public static readonly StyledProperty<string> PathProperty =
        AvaloniaProperty.Register<CertificatePicker, string>(nameof(Path), string.Empty, defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsCertificateSelectedProperty = AvaloniaProperty.Register<CertificatePicker, bool>(nameof(IsCertificateSelected));

    Button? _pickButton;

    public bool IsCertificateSelected
    {
        get => GetValue(IsCertificateSelectedProperty);
        private set => SetValue(IsCertificateSelectedProperty, value);
    }

    public string Path
    {
        get => GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _pickButton = (Button)this.GetTemplateChild("PART_PickButton");
        _pickButton.Click += OnPickButtonClicked;
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        IsCertificateSelected = !string.IsNullOrEmpty(Path);
    }

    void OnPickButtonClicked(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var filePickerOptions = new FilePickerOpenOptions
            {
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Certificates")
                    {
                        Patterns = new[]
                        {
                            "*.pem",
                            "*.crt",
                            "*.pfx"
                        }
                    }
                }
            };
            
            var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(filePickerOptions);
            if (files.Count == 0)
            {
                return;
            }

            Path = files[0].Path.LocalPath;
        });
    }
}