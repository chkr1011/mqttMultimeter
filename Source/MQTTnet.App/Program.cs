using Avalonia;
using Avalonia.ReactiveUI;

namespace MQTTnet.App;

static class Program
{
    public static void Main(string[] args)
    {
        AppBuilder.Configure<App>().UseReactiveUI().UsePlatformDetect().LogToTrace().StartWithClassicDesktopLifetime(args);
    }
}