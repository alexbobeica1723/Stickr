using SQLite;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class StickersRepository : IStickersRepository
{
    private readonly SQLiteAsyncConnection _db;

    public StickersRepository(IDatabaseService databaseService)
    {
        _db = databaseService.GetConnection();
    }

    public Task<List<Sticker>> GetStickersByAlbumIdAsync(string albumId)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId)
            .OrderBy(s => s.Number)
            .ToListAsync();

    public Task InsertStickerAsync(Sticker sticker)
        => _db.InsertAsync(sticker);

    public Task InsertMultipleStickersAsync(IEnumerable<Sticker> stickers)
        => _db.InsertAllAsync(stickers);

    public Task<int> CountStickersAsync(string albumId, int number)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .CountAsync();
    
    public Task<List<Sticker>> GetStickersByAlbumAndNumberAsync(string albumId, int number)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .OrderBy(s => s.AddedAt)
            .ToListAsync();
    
    public async Task<bool> DeleteDuplicateStickerAsync(string albumId, int number)
    {
        var stickers = await GetStickersByAlbumAndNumberAsync(albumId, number);

        // Rule: cannot delete if only one exists
        if (stickers.Count <= 1)
            return false;

        // Delete ONE duplicate (oldest or newest, your choice)
        var stickerToDelete = stickers.Last();
        await _db.DeleteAsync(stickerToDelete);

        return true;
    }
}