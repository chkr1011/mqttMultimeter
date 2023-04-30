using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;
using mqttMultimeter.Extensions;
using mqttMultimeter.Main;

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


    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        IsCertificateSelected = !string.IsNullOrEmpty(Path);
    }

    void OnPickButtonClicked(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Choose certificate",
                Filters = new List<FileDialogFilter>
                {
                    new()
                    {
                        Extensions =
                        {
                            "pem",
                            "crt",
                            "pfx"
                        },
                        Name = "Certificates"
                    }
                }
            };

            var filenames = await openFileDialog.ShowAsync(MainWindow.Instance);
            if (filenames?.Length == 0)
            {
                return;
            }

            Path = filenames?.FirstOrDefault() ?? string.Empty;
        });
    }
}