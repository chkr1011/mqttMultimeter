using System;
using System.IO;
using System.Text;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Publish;

public sealed class PayloadInputFormatSelectorViewModel : BaseViewModel
{
    bool _isBase64String;
    bool _isFilePath;
    bool _isPlainText;

    public PayloadInputFormatSelectorViewModel()
    {
        Value = PayloadInputFormat.PlainText;
    }

    public bool IsBase64String
    {
        get => _isBase64String;
        set => this.RaiseAndSetIfChanged(ref _isBase64String, value);
    }

    public bool IsFilePath
    {
        get => _isFilePath;
        set => this.RaiseAndSetIfChanged(ref _isFilePath, value);
    }

    public bool IsPlainText
    {
        get => _isPlainText;
        set => this.RaiseAndSetIfChanged(ref _isPlainText, value);
    }

    public PayloadInputFormat Value
    {
        get
        {
            if (IsPlainText)
            {
                return PayloadInputFormat.PlainText;
            }

            if (IsBase64String)
            {
                return PayloadInputFormat.Base64String;
            }

            if (IsFilePath)
            {
                return PayloadInputFormat.FilePath;
            }

            throw new NotSupportedException();
        }

        set
        {
            IsPlainText = value == PayloadInputFormat.PlainText;
            IsBase64String = value == PayloadInputFormat.Base64String;
            IsFilePath = value == PayloadInputFormat.FilePath;
        }
    }

    public byte[] ConvertPayloadInput(string? payloadInput)
    {
        if (string.IsNullOrEmpty(payloadInput))
        {
            return Array.Empty<byte>();
        }

        if (_isPlainText)
        {
            return Encoding.UTF8.GetBytes(payloadInput);
        }

        if (_isBase64String)
        {
            return Convert.FromBase64String(payloadInput);
        }

        if (_isFilePath)
        {
            return File.ReadAllBytes(payloadInput);
        }

        throw new NotSupportedException();
    }
}