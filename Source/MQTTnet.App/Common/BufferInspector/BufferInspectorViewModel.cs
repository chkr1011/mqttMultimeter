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
        readonly BufferValueViewModel _offsetValue = new BufferValueViewModel("Offset");
        readonly BufferValueViewModel _bytesLeftValue = new BufferValueViewModel("Bytes left");

        readonly BufferValueViewModel _byteValue = new BufferValueViewModel("Byte");
        readonly BufferValueViewModel _booleanValue = new BufferValueViewModel("Boolean");
        readonly BufferValueViewModel _bitsValue = new BufferValueViewModel("Bits");
        readonly BufferValueViewModel _asciiCharValue = new BufferValueViewModel("ASCII char");

        readonly BufferValueViewModel _int16Value = new BufferValueViewModel("Int 16");
        readonly BufferValueViewModel _uint16Value = new BufferValueViewModel("UInt 16");
        readonly BufferValueViewModel _utf8Char = new BufferValueViewModel("UTF8 char");

        readonly BufferValueViewModel _int32Value = new BufferValueViewModel("Int 32");
        readonly BufferValueViewModel _uint32Value = new BufferValueViewModel("UInt 32");
        readonly BufferValueViewModel _floatValue = new BufferValueViewModel("Float");

        readonly BufferValueViewModel _int64Value = new BufferValueViewModel("Int 64");
        readonly BufferValueViewModel _uint64Value = new BufferValueViewModel("UInt 64");
        readonly BufferValueViewModel _doubleValue = new BufferValueViewModel("Double");
        readonly BufferValueViewModel _unicodeCharValue = new BufferValueViewModel("Unicode char");

        byte[] _buffer;
        string _utf8Content;
        string _jsonContent;
        string _base64Content;
        string _hexContent;
        string _hexPreview;

        public BufferInspectorViewModel()
        {
            Values.Add(_offsetValue);
            Values.Add(_bytesLeftValue);

            Values.Add(_byteValue);
            Values.Add(_booleanValue);
            Values.Add(_bitsValue);
            Values.Add(_asciiCharValue);

            Values.Add(_int16Value);
            Values.Add(_uint16Value);
            Values.Add(_utf8Char);

            Values.Add(_int32Value);
            Values.Add(_uint32Value);
            Values.Add(_floatValue);
            Values.Add(_int64Value);
            Values.Add(_uint64Value);
            Values.Add(_doubleValue);
            Values.Add(_unicodeCharValue);
        }

        public byte[] Buffer
        {
            get => _buffer;
            private set
            {
                _buffer = value;
                OnPropertyChanged();
            }
        }

        public void Dump(byte[] buffer)
        {
            Buffer = buffer;



            DumpAsHex(buffer);

            HexCaretIndex = 0;

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

        void DumpAsHex(byte[] buffer)
        {
            var column = 0;
            var contentBuilder = new StringBuilder();
            var previewBuilder = new StringBuilder();

            foreach (var @byte in buffer)
            {
                var byteHex = BitConverter.ToString(new[] { @byte });

                contentBuilder.Append(byteHex);
                contentBuilder.Append(' ');

                if (@byte > 0x0020 && @byte < 0x007F)
                {
                    previewBuilder.Append((char)@byte);
                }
                else
                {
                    previewBuilder.Append('.');
                }

                column++;

                if (column == 16)
                {
                    contentBuilder.AppendLine();
                    column = 0;

                    previewBuilder.AppendLine();
                }
            }

            HexContent = contentBuilder.ToString();
            HexPreview = previewBuilder.ToString();
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

            foreach (var value in Values)
            {
                value.SetValue(string.Empty);
            }

            _offsetValue.SetValue(_buffer.Length - buffer.Length);
            _bytesLeftValue.SetValue(buffer.Length);

            _byteValue.SetValue(buffer[0]);
            _booleanValue.SetValue(buffer[0] > 0);
            _bitsValue.SetValue(GetBits(buffer[0]));
            _asciiCharValue.SetValue((char)buffer[0]);

            if (buffer.Length < 2)
            {
                return;
            }

            _int16Value.SetValue(BitConverter.ToInt16(buffer));
            _uint16Value.SetValue(BitConverter.ToUInt16(buffer));
            _utf8Char.SetValue(Encoding.UTF8.GetString(buffer, 0, 2));

            if (buffer.Length < 4)
            {
                return;
            }

            _int32Value.SetValue(BitConverter.ToInt32(buffer));
            _uint32Value.SetValue(BitConverter.ToUInt32(buffer));
            _floatValue.SetValue(BitConverter.ToSingle(buffer));

            if (buffer.Length < 8)
            {
                return;
            }

            _int64Value.SetValue(BitConverter.ToInt64(buffer));
            _uint64Value.SetValue(BitConverter.ToUInt64(buffer));
            _doubleValue.SetValue(BitConverter.ToDouble(buffer));
            _unicodeCharValue.SetValue(Encoding.Unicode.GetString(buffer, 0, 4));
        }

        static string GetBits(byte @byte)
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
            get => _hexContent;
            set
            {
                _hexContent = value;
                OnPropertyChanged();
            }
        }

        public string HexPreview
        {
            get => _hexPreview;
            set
            {
                _hexPreview = value;
                OnPropertyChanged();
            }
        }

        public string Utf8Content
        {
            get => _utf8Content;
            set
            {
                _utf8Content = value;
                OnPropertyChanged();
            }
        }

        public string JsonContent
        {
            get => _jsonContent;
            set
            {
                _jsonContent = value;
                OnPropertyChanged();
            }
        }

        public string Base64Content
        {
            get => _base64Content;
            set
            {
                _base64Content = value;
                OnPropertyChanged();
            }
        }
    }
}