using System;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class SessionOptionsViewModel : BaseViewModel
{
    public SessionOptionsViewModel()
    {
        ClientId = "mqttMultimeter-" + Guid.NewGuid();
        KeepAliveInterval = 10;
        CleanSession = true;

        AuthenticationMethod = string.Empty;
        AuthenticationData = string.Empty;
        
        UserName = string.Empty;
        Password = string.Empty;

        CertificatePath = string.Empty;
        CertificatePassword = string.Empty;
    }

    public string AuthenticationData
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string AuthenticationMethod
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string CertificatePassword
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string CertificatePath
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool CleanSession
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string ClientId
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public int KeepAliveInterval
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Password
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool RequestProblemInformation
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool RequestResponseInformation
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool SaveCertificatePassword
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public int SessionExpiryInterval
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string UserName
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();
}