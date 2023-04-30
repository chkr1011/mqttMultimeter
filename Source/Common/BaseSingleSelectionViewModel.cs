using ReactiveUI;

namespace mqttMultimeter.Common;

public abstract class BaseSingleSelectionViewModel : BaseViewModel
{
    readonly bool[] _states;

    protected BaseSingleSelectionViewModel(int count)
    {
        _states = new bool[count];
    }

    protected bool GetState(int index)
    {
        return _states[index];
    }

    protected void UpdateStates(int changedStateIndex, bool changedStateValue)
    {
        if (!changedStateValue)
        {
            // Do not allow "unchecking" the value.
            return;
        }

        for (var i = 0; i < _states.Length; i++)
        {
            if (i == changedStateIndex)
            {
                _states[i] = changedStateValue;
            }
            else
            {
                _states[i] = !changedStateValue;
            }
        }

        // Notify all properties!
        this.RaisePropertyChanged(string.Empty);
    }
}