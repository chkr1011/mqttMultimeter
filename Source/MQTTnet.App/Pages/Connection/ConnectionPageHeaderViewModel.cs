using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class ConnectionPageHeaderViewModel : BaseViewModel
    {
        public string Title { get; } = "Connection";

        public bool IsConnected
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

    }
}
