using Stickr.Services.Implementations;

namespace Stickr;

public partial class App : Application
{
    private readonly IServiceProvider _services;
    
    public App(IServiceProvider services)
    {
        InitializeComponent();

        MainPage = new AppShell();
        _services = services;
        
        // Fire-and-forget async initialization
        InitializeAsync();
    }
    
    private async void InitializeAsync()
    {
        try
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseService>();
            await db.InitializeAsync();

            var seeder = scope.ServiceProvider.GetRequiredService<SeedService>();
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            // Log if something fails
            Console.WriteLine($"Initialization error: {ex}");
        }
    }
}