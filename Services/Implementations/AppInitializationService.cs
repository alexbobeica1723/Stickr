namespace Stickr.Services.Implementations;

public class AppInitializationService
{
    private readonly DatabaseService _databaseService;
    private readonly SeedService _seedService;

    private readonly TaskCompletionSource<bool> _readyTcs = new();

    public Task Ready => _readyTcs.Task;

    public AppInitializationService(
        DatabaseService databaseService,
        SeedService seedService)
    {
        _databaseService = databaseService;
        _seedService = seedService;
    }

    public async Task InitializeAsync()
    {
        await _databaseService.InitializeAsync();
        await _seedService.SeedAsync();

        _readyTcs.TrySetResult(true);
    }
}