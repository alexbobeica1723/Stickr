namespace Stickr.ViewModels.Base;

public abstract class BaseModalPageViewModel : BaseViewModel
{
    /// <summary>
    /// Called once when the page appears for the first time
    /// </summary>
    /*public async Task InitializeAsync()
    {
        await InitializeDataAsync();
    }*/

    /// <summary>
    /// Each page ViewModel must implement its own data loading logic
    /// </summary>
    public abstract Task InitializeDataAsync();
}