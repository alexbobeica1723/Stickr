using CommunityToolkit.Mvvm.ComponentModel;

namespace Stickr.ViewModels.Base;

public partial class BaseViewModel : ObservableObject
{
    #region Properties
    
    [ObservableProperty]
    private bool _isBusy;
    
    #endregion
}