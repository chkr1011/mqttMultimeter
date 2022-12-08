using System;

namespace mqttMultimeter.Services.State;

public sealed class SavingStateEventArgs : EventArgs
{
    public SavingStateEventArgs(StateService stateService)
    {
        StateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
    }

    public StateService StateService { get; }
}