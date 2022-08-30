using System.Text;

namespace MQTTnetApp.Controls;

public static class BufferPreviewTestData
{
    public static byte[] Payload => Encoding.UTF8.GetBytes("Demo Payload");
}