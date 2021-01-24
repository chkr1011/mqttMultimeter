using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace MQTTnet.App.Common
{
    public sealed class BufferInspectorViewModel : BaseViewModel
    {
        readonly byte[] _buffer;

        public BufferInspectorViewModel(byte[] buffer)
        {
            _buffer = buffer;

            var column = 0;
            var contentBuilder = new StringBuilder();

            foreach (var @byte in buffer)
            {
                var byteHex = BitConverter.ToString(new[] { @byte });

                contentBuilder.Append(byteHex);
                contentBuilder.Append(" ");

                column++;

                if (column == 16)
                {
                    contentBuilder.AppendLine();
                    column = 0;
                }
            }

            HexContent = contentBuilder.ToString();

            Utf8Content = Encoding.UTF8.GetString(buffer);

            Base64Content = Convert.ToBase64String(buffer);

            try
            {
                var json = JObject.Parse(Encoding.UTF8.GetString(buffer));
                JsonContent = json.ToString(Formatting.Indented);
            }
            catch (Exception exception)
            {
                JsonContent = exception.Message;
            }
        }

        int _hexCaretIndex;

        public int HexCaretIndex
        {
            get => _hexCaretIndex;
            set
            {
                _hexCaretIndex = value;

                TryParseValuesFromHexOffset();
            }
        }

        void TryParseValuesFromHexOffset()
        {
            var hexContent = HexContent;

            int i = _hexCaretIndex;
            if (i + 1 > hexContent.Length - 1)
            {
                return;
            }

            if (hexContent[i + 1] == ' ')
            {
                i--;
            }

            var source = hexContent.Substring(i);
            source = source.Replace(Environment.NewLine, " ");
            source = source.Trim();
            source = source.Replace(" ", "");

            var buffer = StringToByteArray(source);

            var parsedHexValuesBuilder = new StringBuilder();
            parsedHexValuesBuilder.AppendLine("byte\t: " + buffer[0]);
            parsedHexValuesBuilder.AppendLine("boolean:\t" + BitConverter.ToBoolean(buffer));
            parsedHexValuesBuilder.AppendLine("int16:\t" + BitConverter.ToInt16(buffer));
            parsedHexValuesBuilder.AppendLine("uint16:\t" + BitConverter.ToUInt16(buffer));
            parsedHexValuesBuilder.AppendLine("int32:\t" + BitConverter.ToInt32(buffer));
            parsedHexValuesBuilder.AppendLine("uint32:\t" + BitConverter.ToUInt32(buffer));
            parsedHexValuesBuilder.AppendLine("float:\t" + BitConverter.ToSingle(buffer));
            parsedHexValuesBuilder.AppendLine("double:\t" + BitConverter.ToDouble(buffer));



            ParsedHexValues = parsedHexValuesBuilder.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        public string HexContent
        {
            get;
        }

        public string ParsedHexValues
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string Utf8Content
        {
            get;
        }

        public string JsonContent
        {
            get;
        }

        public string Base64Content
        {
            get;
        }
    }
}