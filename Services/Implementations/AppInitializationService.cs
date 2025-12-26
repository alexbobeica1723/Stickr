using Plugin.Maui.OCR;
using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class AppInitializationService : IAppInitializationService
{
    #region Fields
    
    private readonly TaskCompletionSource<bool> _readyTcs = new();
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly IDatabaseService _databaseService;

    public AppInitializationService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    
    #endregion

    #region Public Methods
    
    public async Task InitializeAsync()
    {
        await _databaseService.InitializeAsync();
        await OcrPlugin.Default.InitAsync();
        _readyTcs.TrySetResult(true);
    }
    
    public Task CompleteInitializationAsync() => _readyTcs.Task;
    
    #endregion
}