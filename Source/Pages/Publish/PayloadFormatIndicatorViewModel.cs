using System;
using System.IO;
using System.Text;
using MQTTnet.Protocol;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Publish;

public sealed class PayloadFormatIndicatorViewModel : BaseViewModel
{
    bool _isCharacterData;
    bool _isUnspecified = true;
    bool _isUnspecifiedBase64String;
    bool _isUnspecifiedFilePath;

    public bool IsCharacterData
    {
        get => _isCharacterData;
        set => this.RaiseAndSetIfChanged(ref _isCharacterData, value);
    }

    public bool IsUnspecified
    {
        get => _isUnspecified;
        set => this.RaiseAndSetIfChanged(ref _isUnspecified, value);
    }

    public bool IsUnspecifiedBase64String
    {
        get => _isUnspecifiedBase64String;
        set => this.RaiseAndSetIfChanged(ref _isUnspecifiedBase64String, value);
    }

    public bool IsUnspecifiedFilePath
    {
        get => _isUnspecifiedFilePath;
        set => this.RaiseAndSetIfChanged(ref _isUnspecifiedFilePath, value);
    }

    public byte[] ToPayload(string? payloadInput)
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