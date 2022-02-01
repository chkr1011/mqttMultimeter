using System;
using MQTTnet.App.Common;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class SessionOptionsViewModel : BaseViewModel
{
    bool _cleanSession = true;
    string _clientId = string.Empty;
    int _keepAliveInterval = 10;
    string _password = string.Empty;
    bool _requestProblemInformation;
    bool _requestResponseInformation;
    string _user = string.Empty;

    public SessionOptionsViewModel()
    {
        ClientId = "MQTTnet.App-" + Guid.NewGuid();
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

    public string User
    {
        get => _user;
        set => this.RaiseAndSetIfChanged(ref _user, value);
    }
}