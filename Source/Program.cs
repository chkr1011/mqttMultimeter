using System.Diagnostics;
using Avalonia;
using Avalonia.ReactiveUI;

namespace MQTTnetApp;

static class Program
{
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Do not remove this method! It is required for the Designer.
    static AppBuilder BuildAvaloniaApp()
    {
        if (Debugger.IsAttached)
        {
            return AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect().LogToTrace();
        }
        
        return AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect();
    }
}