namespace Stickr.ViewModels.Base;

public abstract class BaseModalPageViewModel : BaseViewModel
{
    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
}