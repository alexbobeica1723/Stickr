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
        
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "stickr.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));
        
        builder
            .UseMauiApp<App>()
            .UseOcr()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<Stickr.Services.Interfaces.IOcrService, OcrService>();
        builder.Services.AddSingleton<SeedService>();

        builder.Services.AddSingleton<CollectionsViewModel>();
        builder.Services.AddSingleton<MyAlbumsViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();

        builder.Services.AddTransient<CollectionsView>();
        builder.Services.AddTransient<MyAlbumsView>();
        builder.Services.AddTransient<ProfileView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}