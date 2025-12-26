using Plugin.Maui.OCR;
using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class AppInitializationService : IAppInitializationService
{
    private readonly IDatabaseService _databaseService;

    private readonly TaskCompletionSource<bool> _readyTcs = new();

    public Task CompleteInitializationAsync() => _readyTcs.Task;

    public AppInitializationService(
        IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task InitializeAsync()
    {
        await _databaseService.InitializeAsync();
        await OcrPlugin.Default.InitAsync();

        _readyTcs.TrySetResult(true);
    }
}