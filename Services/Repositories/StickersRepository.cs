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
}