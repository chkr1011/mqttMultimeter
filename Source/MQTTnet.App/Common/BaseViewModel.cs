using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MQTTnet.App.Common
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        readonly ViewModelPropertyStore _propertyStore = new ViewModelPropertyStore();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetValue<TValue>(TValue value, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            if (_propertyStore.SetValue(propertyName, value))
            {
                //this.RaisePropertyChanged(propertyName);
                OnPropertyChanged(propertyName);
            }
        }

        protected TValue GetValue<TValue>([CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            return _propertyStore.GetValue<TValue>(propertyName);
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
