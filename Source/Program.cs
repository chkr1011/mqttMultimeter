using Avalonia;
using Avalonia.ReactiveUI;

namespace MQTTnetApp;

static class Program
{
    public static void Main(string[] args)
    {
        AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect().LogToTrace().StartWithClassicDesktopLifetime(args);
    }
}