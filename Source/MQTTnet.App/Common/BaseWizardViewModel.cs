namespace MQTTnet.App.Common
{
    public abstract class BaseWizardViewModel : BaseViewModel
    {
        BaseViewModel? _activePage;

        public BaseViewModel? ActivePage
        {
            get => _activePage;
            protected set
            {
                _activePage = value;
                OnPropertyChanged();
            }
        }
    }
}