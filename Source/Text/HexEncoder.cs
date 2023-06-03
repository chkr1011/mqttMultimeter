using System.Text;

namespace mqttMultimeter.Text;

public static class HexEncoder
{
    public static string GetString(byte[]? buffer)
    {
        if (buffer == null)
        {
            return string.Empty;
        }

        if (buffer.Length == 0)
        {
            return string.Empty;
        }

        var output = new StringBuilder(buffer.Length * 2);
        foreach (var @byte in buffer)
        {
            output.Append($"{@byte:x2}");
        }

        return output.ToString();
    }
}