using Plugin.Maui.OCR;
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
    }
}