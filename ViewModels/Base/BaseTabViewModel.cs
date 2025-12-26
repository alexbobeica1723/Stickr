using Stickr.Services.Interfaces;

namespace Stickr.ViewModels.Base;

public abstract class BaseTabViewModel : BaseViewModel
{
    #region Constructor & Dependencies
    
    private readonly IAppInitializationService _appInitializationService;
    
    protected BaseTabViewModel(IAppInitializationService appInit)
    {
        _appInitializationService = appInit;
    }
    
    #endregion
    
    #region Abstract Methods

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
    
    #endregion
}