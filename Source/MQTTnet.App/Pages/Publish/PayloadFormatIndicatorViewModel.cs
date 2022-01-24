using System;
using System.IO;
using System.Text;
using MQTTnet.App.Common;
using MQTTnet.Protocol;

namespace MQTTnet.App.Pages.Publish;

public sealed class PayloadFormatIndicatorViewModel : BaseViewModel
{
    public bool IsCharacterData
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsUnspecified
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsUnspecifiedBase64String
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsUnspecifiedFilePath
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public byte[] ToPayload(string payloadInput)
    {
        if (string.IsNullOrEmpty(payloadInput))
        {
            return Array.Empty<byte>();
        }

        if (IsUnspecified)
        {
            // Also use the encoded UTF8 string here because this not able to get input in other
            // ways like HEX editors etc.
            return Encoding.UTF8.GetBytes(payloadInput);
        }

        if (IsCharacterData)
        {
            return Encoding.UTF8.GetBytes(payloadInput);
        }

        if (IsUnspecifiedBase64String)
        {
            return Convert.FromBase64String(payloadInput);
        }

        if (IsUnspecifiedFilePath)
        {
            return File.ReadAllBytes(payloadInput);
        }

        throw new NotSupportedException();
    }

    public MqttPayloadFormatIndicator ToPayloadFormatIndicator()
    {
        if (IsCharacterData)
        {
            return MqttPayloadFormatIndicator.CharacterData;
        }

        return MqttPayloadFormatIndicator.Unspecified;
    }
}