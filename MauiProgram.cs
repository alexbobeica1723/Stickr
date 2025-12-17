using Microsoft.Extensions.Logging;
using Plugin.Maui.OCR;
using Stickr.Services.Implementations;
using Stickr.Services.Repositories;
using Stickr.ViewModels;
using Stickr.ViewModels.Pages;
using Stickr.Views;
using Stickr.Views.Pages;

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
        
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "stickr.db3");

        // Services
        builder.Services.AddSingleton(new DatabaseService(dbPath));
        builder.Services.AddSingleton<CollectionsRepository>();
        builder.Services.AddSingleton<AlbumsRepository>();
        builder.Services.AddSingleton<StickersRepository>();
        builder.Services.AddSingleton<SeedService>();
        builder.Services.AddSingleton<AppInitializationService>();
        builder.Services.AddSingleton<Stickr.Services.Interfaces.IOcrService, OcrService>();

        // ViewModels
        builder.Services.AddSingleton<CollectionsViewModel>();
        builder.Services.AddSingleton<MyAlbumsViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<AlbumDetailsViewModel>();

        // Views
        builder.Services.AddTransient<CollectionsView>();
        builder.Services.AddTransient<MyAlbumsView>();
        builder.Services.AddTransient<ProfileView>();
        builder.Services.AddTransient<AlbumDetailsView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}