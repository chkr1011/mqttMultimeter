using System;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class SessionOptionsViewModel : BaseViewModel
    {
        public SessionOptionsViewModel()
        {
            ClientId = "MQTTnet.App-" + Guid.NewGuid();
            KeepAliveInterval = 10;
            CleanSession = true;
            RequestProblemInformation = false;
        }

        public string ClientId
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string User
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Password
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int KeepAliveInterval
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public bool CleanSession
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool RequestProblemInformation
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool RequestResponseInformation
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}