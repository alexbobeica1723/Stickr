using CommunityToolkit.Mvvm.ComponentModel;
using Stickr.Services.Implementations;

namespace Stickr.ViewModels.Base;

public abstract class BasePageViewModel : BaseViewModel
{
    protected readonly AppInitializationService AppInit;
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
    
    protected BasePageViewModel(AppInitializationService appInit)
    {
        AppInit = appInit;
    }

    /// <summary>
    /// Called once when the page appears for the first time
    /// </summary>
    public async Task InitializeAsync()
    {
        await AppInit.Ready;
        await InitializeDataAsync();
    }

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
}