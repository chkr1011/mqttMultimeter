using System.Diagnostics;
using System.IO;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace mqttMultimeter.Pages.Info;

public sealed partial class InfoPageView : UserControl
{
    public InfoPageView()
    {
        InitializeComponent();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var licenses = this.FindControl<TextBlock>("Licenses")!;
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

    void OnLatestVersionClicked(object? sender, PointerPressedEventArgs e)
    {
        ((InfoPageViewModel)DataContext!).OpenReleasesUrl();
    }

    void OnOpenHomepage(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/mqttMultimeter");
    }

    void OnReportBug(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/mqttMultimeter/issues/new");
    }

    void OnRequestFeature(object? sender, RoutedEventArgs e)
    {
        Launch("https://github.com/chkr1011/mqttMultimeter/issues/new");
    }

    void OpenUrlFromButtonContent(object? sender, RoutedEventArgs e)
    {
        Launch((sender as Button)?.Content as string ?? string.Empty);
    }

    static string ReadEmbeddedMarkdown()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("mqttMultimeter.Pages.Info.Readme.md");

        if (stream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}