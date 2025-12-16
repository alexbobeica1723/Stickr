namespace Stickr.Services.Implementations;

public class AppInitializationService
{
    private readonly DatabaseService _db;
    private readonly SeedService _seed;

    private readonly TaskCompletionSource<bool> _readyTcs = new();

    public Task Ready => _readyTcs.Task;

    public AppInitializationService(
        DatabaseService db,
        SeedService seed)
    {
        _db = db;
        _seed = seed;
    }

    public async Task InitializeAsync()
    {
        await _db.InitializeAsync();
        await _seed.SeedAsync();

        _readyTcs.TrySetResult(true);
    }
}