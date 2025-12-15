namespace Stickr.ViewModels.Base;

public abstract class BasePageViewModel : BaseViewModel
{
    private bool _isBusy;
    private bool _isInitialized;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value) return;
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Called once when the page appears for the first time
    /// </summary>
    public async Task InitializeAsync()
    {
        if (_isInitialized)
            return;

        try
        {
            IsBusy = true;
            await InitializeDataAsync();
            _isInitialized = true;
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    protected abstract Task InitializeDataAsync();
}