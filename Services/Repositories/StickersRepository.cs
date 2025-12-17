using SQLite;
using Stickr.Models;
using Stickr.Services.Implementations;

namespace Stickr.Services.Repositories;

public class StickersRepository
{
    private readonly SQLiteAsyncConnection _db;

    public StickersRepository(DatabaseService databaseService)
    {
        _db = databaseService.GetConnection();
    }

    public Task<List<Sticker>> GetByAlbumIdAsync(string albumId)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId)
            .OrderBy(s => s.Number)
            .ToListAsync();

    public Task InsertAsync(Sticker sticker)
        => _db.InsertAsync(sticker);

    public Task InsertManyAsync(IEnumerable<Sticker> stickers)
        => _db.InsertAllAsync(stickers);

    public Task<int> CountAsync(string albumId, int number)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .CountAsync();
    
    private Task<List<Sticker>> GetByAlbumAndNumberAsync(string albumId, int number)
        => _db.Table<Sticker>()
            .Where(s => s.AlbumId == albumId && s.Number == number)
            .OrderBy(s => s.AddedAt)
            .ToListAsync();
    
    public async Task<bool> DeleteOneDuplicateAsync(string albumId, int number)
    {
        var stickers = await GetByAlbumAndNumberAsync(albumId, number);

        // Rule: cannot delete if only one exists
        if (stickers.Count <= 1)
            return false;

        // Delete ONE duplicate (oldest or newest, your choice)
        var stickerToDelete = stickers.Last();
        await _db.DeleteAsync(stickerToDelete);

        return true;
    }
}