using System.Diagnostics;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace MQTTnet.App.Pages.Info;

public sealed class InfoPageView : UserControl
{
    public InfoPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var licenses = this.FindControl<TextBlock>("Licenses");
        licenses.Text = ReadEmbeddedMarkdown();
    }

    static void Launch(string fileName)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        });
    }

    void OnOpenHomepage(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/MQTTnet.App");
    }

    void OpenUrlFromButtonContent(object? sender, RoutedEventArgs e)
    {
        Launch(((Button)sender).Content as string);
    }
    
    void OnReportBug(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/MQTTnet.App/issues/new");
    }

    void OnRequestFeature(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/MQTTnet.App/issues/new");
    }

    static string ReadEmbeddedMarkdown()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("MQTTnet.App.Pages.Info.Readme.md");

        if (stream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}