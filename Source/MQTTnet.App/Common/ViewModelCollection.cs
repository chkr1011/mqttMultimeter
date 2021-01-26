using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MQTTnet.App.Common
{
    public sealed class ViewModelCollection<TItem> : ObservableCollection<TItem>
    {
        TItem _selectedItem = default!;

        public TItem SelectedItem
        {
            get => _selectedItem;

            set
            {
                _selectedItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }
    }
}