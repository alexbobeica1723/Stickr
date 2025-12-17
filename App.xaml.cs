using Plugin.Maui.OCR;
using Stickr.Services.Implementations;
using Stickr.Views.Pages;

namespace Stickr;

public partial class App : Application
{
    public App(AppInitializationService initService)
    {
        InitializeComponent();

        MainPage = new StartupPage();

        _ = InitializeAsync(initService);
    }

    private async Task InitializeAsync(AppInitializationService initService)
    {
        await initService.InitializeAsync();
        await OcrPlugin.Default.InitAsync();

        MainPage = new AppShell();
    }
}