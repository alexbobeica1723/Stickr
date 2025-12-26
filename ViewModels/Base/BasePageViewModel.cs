using Stickr.Services.Implementations;
using Stickr.Services.Interfaces;

namespace Stickr.ViewModels.Base;

public abstract class BasePageViewModel : BaseViewModel
{
    private readonly IAppInitializationService _appInitializationService;
    
    protected BasePageViewModel(IAppInitializationService appInit)
    {
        _appInitializationService = appInit;
    }

    /// <summary>
    /// Called once when the page appears for the first time
    /// </summary>
    public async Task InitializeAsync()
    {
        await _appInitializationService.CompleteInitializationAsync();
        await InitializeDataAsync();
    }

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
}