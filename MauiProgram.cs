using Microsoft.Extensions.Logging;
using Plugin.Maui.OCR;
using Stickr.Repositories.Implementations;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Implementations;
using Stickr.Services.Interfaces;
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

        // Services
        builder.Services.AddSingleton<IAppInitializationService, AppInitializationService>();
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
        builder.Services.AddSingleton<IDisplayAlertService, DisplayAlertService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IPermissionsService, PermissionsService>();
        builder.Services.AddSingleton<Services.Interfaces.IOcrService, OcrService>();
        
        // Repositories
        builder.Services.AddSingleton<ICollectionsRepository, CollectionsRepository>();
        builder.Services.AddSingleton<IAlbumsRepository, AlbumsRepository>();
        builder.Services.AddSingleton<IStickersRepository, StickersRepository>();

        // ViewModels
        builder.Services.AddSingleton<CollectionsViewModel>();
        builder.Services.AddSingleton<MyAlbumsViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddTransient<AlbumDetailsViewModel>();
        builder.Services.AddTransient<CreateCollectionViewModel>();
        builder.Services.AddTransient<StickerDetailsViewModel>();
        builder.Services.AddTransient<AlbumStatsViewModel>();
        builder.Services.AddSingleton<OnboardingViewModel>();

        // Views
        builder.Services.AddSingleton<CollectionsView>();
        builder.Services.AddSingleton<MyAlbumsView>();
        builder.Services.AddSingleton<ProfileView>();
        builder.Services.AddTransient<AlbumDetailsView>();
        builder.Services.AddTransient<CreateCollectionView>();
        builder.Services.AddTransient<StickerDetailsView>();
        builder.Services.AddTransient<AlbumStatsView>();
        builder.Services.AddSingleton<OnboardingView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}