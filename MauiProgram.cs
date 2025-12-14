using Microsoft.Extensions.Logging;
using Plugin.Maui.OCR;
using Stickr.Services.Implementations;
using Stickr.ViewModels;
using Stickr.Views;

namespace Stickr;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseOcr()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Stickr.Services.Interfaces.IOcrService, OcrService>();

        builder.Services.AddTransient<CollectionsViewModel>();
        builder.Services.AddTransient<MyAlbumsViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        builder.Services.AddTransient<CollectionsView>();
        builder.Services.AddTransient<MyAlbumsView>();
        builder.Services.AddTransient<ProfileView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}