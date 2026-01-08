using SQLite;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class StickersRepository : IStickersRepository
{
    #region Constructor & Dependencies
    
    private readonly SQLiteAsyncConnection _databaseConnection;

    public StickersRepository(IDatabaseService databaseService)
    {
        _databaseConnection = databaseService.GetConnection();
    }
    
    #endregion
    
    #region Public Methods

    public Task<List<Sticker>> GetStickersByCollectionIdAsync(string collectionId)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.CollectionId == collectionId)
            .OrderBy(s => s.Number)
            .ToListAsync();

    public Task InsertStickerAsync(Sticker sticker) => _databaseConnection.InsertAsync(sticker);

    public Task InsertMultipleStickersAsync(IEnumerable<Sticker> stickers) 
        => _databaseConnection.InsertAllAsync(stickers);

    public Task<int> CountCollectionStickersAsync(string collectionId, int number)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.CollectionId == collectionId && s.Number == number)
            .CountAsync();
    
    public Task<List<Sticker>> GetStickersByCollectionAndNumberAsync(string collectionId, int number)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.CollectionId == collectionId && s.Number == number)
            .OrderBy(s => s.AddedAt)
            .ToListAsync();
    
    public async Task<int> GetUniqueStickerCountAsync(string collectionId)
    {
        var numbers = await _databaseConnection.Table<Sticker>()
            .Where(s => s.CollectionId == collectionId)
            .ToListAsync();

        return numbers.Distinct().Count();
    }
    
    public async Task<bool> DeleteDuplicatedStickerAsync(string collectionId, int number)
    {
        var stickers = await GetStickersByCollectionAndNumberAsync(collectionId, number);

        // Rule: don't delete if only one sticker exists as it is considered as already added to the album
        if (stickers.Count <= 1)
        {
            return false;
        }
        
        var stickerToDelete = stickers.Last();
        await _databaseConnection.DeleteAsync(stickerToDelete);

        return true;
    }
    
    #endregion
}