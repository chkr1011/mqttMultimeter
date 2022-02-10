using System;
using MQTTnetApp.Common;
using MQTTnetApp.Controls;
using ReactiveUI;

namespace MQTTnetApp.Pages.Connection;

public sealed class SessionOptionsViewModel : BaseViewModel
{
    string _authenticationData = string.Empty;
    string _authenticationMethod = string.Empty;
    bool _cleanSession = true;
    string _clientId = string.Empty;
    int _keepAliveInterval = 10;
    string _password = string.Empty;
    bool _requestProblemInformation;
    bool _requestResponseInformation;
    int _sessionExpiryInterval;
    string _userName = string.Empty;

    public SessionOptionsViewModel()
    {
        ClientId = "MQTTnetApp-" + Guid.NewGuid();
    }

    public string AuthenticationData
    {
        get => _authenticationData;
        set => this.RaiseAndSetIfChanged(ref _authenticationData, value);
    }

    public string AuthenticationMethod
    {
        get => _authenticationMethod;
        set => this.RaiseAndSetIfChanged(ref _authenticationMethod, value);
    }

    public bool CleanSession
    {
        get => _cleanSession;
        set => this.RaiseAndSetIfChanged(ref _cleanSession, value);
    }

    public string ClientId
    {
        get => _clientId;
        set => this.RaiseAndSetIfChanged(ref _clientId, value);
    }

    public int KeepAliveInterval
    {
        get => _keepAliveInterval;
        set => this.RaiseAndSetIfChanged(ref _keepAliveInterval, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public bool RequestProblemInformation
    {
        get => _requestProblemInformation;
        set => this.RaiseAndSetIfChanged(ref _requestProblemInformation, value);
    }

    public bool RequestResponseInformation
    {
        get => _requestResponseInformation;
        set => this.RaiseAndSetIfChanged(ref _requestResponseInformation, value);
    }

    public int SessionExpiryInterval
    {
        get => _sessionExpiryInterval;
        set => this.RaiseAndSetIfChanged(ref _sessionExpiryInterval, value);
    }

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();
}