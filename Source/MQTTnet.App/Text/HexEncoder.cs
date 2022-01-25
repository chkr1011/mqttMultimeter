using System.Text;

namespace MQTTnet.App.Text;

public static class HexEncoder
{
    public static string GetString(byte[] buffer)
    {
        if (buffer.Length == 0)
        {
            return string.Empty;
        }

        var output = new StringBuilder(buffer.Length * 2);
        foreach (var @byte in buffer)
        {
            output.AppendFormat("{0:x2}", @byte);
        }

        return output.ToString();
    }
}