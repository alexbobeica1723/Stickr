using Stickr.Services.Interfaces;
using Stickr.Views.Pages;

namespace Stickr;

public partial class App : Application
{
    public App(IAppInitializationService initService)
    {
        InitializeComponent();

        MainPage = new StartupPage();

        _ = InitializeAsync(initService);
    }

    private async Task InitializeAsync(IAppInitializationService initService)
    {
        await initService.InitializeAsync();

        MainPage = new AppShell();

        var savedTheme = Preferences.Get("AppTheme", string.Empty);

        if (Enum.TryParse<AppTheme>(savedTheme, out var theme))
        {
            Application.Current.UserAppTheme = theme;
        }
        
        var completed = Preferences.Get("OnboardingCompleted", false);

        if (!completed)
        {
            await MainPage.Dispatcher.DispatchAsync(async () =>
            {
                await Shell.Current.Navigation.PushModalAsync(
                    new OnboardingView());
            });
        }
    }
}