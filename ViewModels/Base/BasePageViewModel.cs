using Stickr.Services.Implementations;

namespace Stickr.ViewModels.Base;

public abstract class BasePageViewModel : BaseViewModel
{
    private readonly AppInitializationService _appInitializationService;
    
    protected BasePageViewModel(AppInitializationService appInit)
    {
        _appInitializationService = appInit;
    }

    /// <summary>
    /// Called once when the page appears for the first time
    /// </summary>
    public async Task InitializeAsync()
    {
        await _appInitializationService.Ready;
        await InitializeDataAsync();
    }

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
}