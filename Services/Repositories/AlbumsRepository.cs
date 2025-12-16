using SQLite;
using Stickr.Models;
using Stickr.Services.Implementations;

namespace Stickr.Services.Repositories;

public class AlbumsRepository
{
    private readonly SQLiteAsyncConnection _db;

    public AlbumsRepository(DatabaseService dbService)
    {
        _db = dbService.GetConnection();
    }

    public Task<List<Album>> GetAllAsync() => _db.Table<Album>().ToListAsync();
    public Task InsertAsync(Album album) => _db.InsertAsync(album);
    public Task<int> UpdateAsync(Album album) => _db.InsertOrReplaceAsync(album);
}