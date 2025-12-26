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

    public Task<List<Sticker>> GetStickersByAlbumIdAsync(string albumId)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.AlbumId == albumId)
            .OrderBy(s => s.Number)
            .ToListAsync();

    public Task InsertStickerAsync(Sticker sticker) => _databaseConnection.InsertAsync(sticker);

    public Task InsertMultipleStickersAsync(IEnumerable<Sticker> stickers) 
        => _databaseConnection.InsertAllAsync(stickers);

    public Task<int> CountStickersAsync(string albumId, int number)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .CountAsync();
    
    public Task<List<Sticker>> GetStickersByAlbumAndNumberAsync(string albumId, int number)
        => _databaseConnection.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .OrderBy(s => s.AddedAt)
            .ToListAsync();
    
    public async Task<bool> DeleteDuplicatedStickerAsync(string albumId, int number)
    {
        var stickers = await GetStickersByAlbumAndNumberAsync(albumId, number);

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