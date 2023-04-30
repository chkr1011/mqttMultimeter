using System.Text;

namespace mqttMultimeter.Controls;

public static class BufferPreviewTestData
{
    public static byte[] Payload => Encoding.UTF8.GetBytes("Demo Payload");

    public static string PayloadString => "Demo Payload";
}