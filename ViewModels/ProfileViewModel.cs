using CommunityToolkit.Mvvm.Input;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    public string Title => "My Albums";
    
    [RelayCommand]
    private void ToggleTheme()
    {
        var app = Application.Current;
        if (app == null)
            return;

        var newTheme =
            app.UserAppTheme == AppTheme.Dark
                ? AppTheme.Light
                : AppTheme.Dark;

        app.UserAppTheme = newTheme;

        Preferences.Set("AppTheme", newTheme.ToString());
    }
}