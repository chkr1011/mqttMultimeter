using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTnet.App.Common.BufferInspector
{
    public sealed class BufferInspectorViewModel : BaseViewModel
    {
        readonly byte[] _buffer;

        public BufferInspectorViewModel(byte[]? buffer)
        {
            _buffer = buffer ?? new byte[0];

            var column = 0;
            var contentBuilder = new StringBuilder();

            foreach (var @byte in _buffer)
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

            Utf8Content = Encoding.UTF8.GetString(_buffer);

            Base64Content = Convert.ToBase64String(_buffer);

            try
            {
                var json = JObject.Parse(Encoding.UTF8.GetString(_buffer));
                JsonContent = json.ToString(Formatting.Indented);
            }
            catch (Exception exception)
            {
                JsonContent = exception.Message;
            }
        }

        int _hexCaretIndex;

        public ObservableCollection<BufferValueViewModel> Values { get; } = new ObservableCollection<BufferValueViewModel>();


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

            Values.Clear();

            Values.Add(new BufferValueViewModel("Offset", (_buffer.Length - buffer.Length).ToString()));
            Values.Add(new BufferValueViewModel("Bytes left", buffer.Length.ToString()));

            Values.Add(new BufferValueViewModel("Decimal", buffer[0].ToString()));
            Values.Add(new BufferValueViewModel("Boolean", (buffer[0] > 0).ToString()));
            Values.Add(new BufferValueViewModel("Bits", GetBits(buffer[0])));
            Values.Add(new BufferValueViewModel("ASCII char", ((char)buffer[0]).ToString()));

            if (buffer.Length < 2)
            {
                return;
            }

            Values.Add(new BufferValueViewModel("Int 16", BitConverter.ToInt16(buffer).ToString()));
            Values.Add(new BufferValueViewModel("UInt 16", BitConverter.ToUInt16(buffer).ToString()));
            Values.Add(new BufferValueViewModel("UTF8 char", Encoding.UTF8.GetString(buffer, 0, 2)));

            if (buffer.Length < 4)
            {
                return;
            }

            Values.Add(new BufferValueViewModel("Int 32", BitConverter.ToInt32(buffer).ToString()));
            Values.Add(new BufferValueViewModel("UInt 32", BitConverter.ToUInt32(buffer).ToString()));
            Values.Add(new BufferValueViewModel("Float", BitConverter.ToSingle(buffer).ToString()));

            if (buffer.Length < 8)
            {
                return;
            }

            Values.Add(new BufferValueViewModel("int64", BitConverter.ToInt32(buffer).ToString()));
            Values.Add(new BufferValueViewModel("uint64", BitConverter.ToUInt32(buffer).ToString()));
            Values.Add(new BufferValueViewModel("Double", BitConverter.ToDouble(buffer).ToString()));
        }

        string GetBits(byte @byte)
        {
            var stringBuilder = new StringBuilder();
            var counter = 0;
            for (var i = 7; i >= 0; i--)
            {
                stringBuilder.Append((@byte & 1 << i) > 0 ? "1" : "0");

                counter++;
                if (counter == 4 && i != 0)
                {
                    stringBuilder.Append(".");
                    counter = 0;
                }
            }

            return stringBuilder.ToString();
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