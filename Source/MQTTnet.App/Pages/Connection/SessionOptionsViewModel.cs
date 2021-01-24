using System;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class SessionOptionsViewModel : BaseViewModel
    {
        public SessionOptionsViewModel()
        {
            ClientId = "MQTTnet.App-" + Guid.NewGuid();
            CleanSession = true;
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

        public bool CleanSession
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}