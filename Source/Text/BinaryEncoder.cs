using System.Text;

namespace mqttMultimeter.Text;

public static class BinaryEncoder
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

        var output = new StringBuilder(buffer.Length * 8);
        foreach (var @byte in buffer)
        {
            for (var i = 0; i < 8; i++)
            {
                var flag = (@byte & (1 << i)) > 0;
                output.Append(flag ? '1' : '0');
            }
        }

        return output.ToString();
    }
}