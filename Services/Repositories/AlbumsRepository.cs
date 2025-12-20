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

    public Task<Album?> GetByCollectionIdAsync(string collectionId)
        => _db.Table<Album>()
            .Where(a => a.CollectionId == collectionId)
            .FirstOrDefaultAsync();
    
    public Task<Album?> GetByIdAsync(string albumId)
        => _db.Table<Album>()
            .Where(a => a.Id == albumId)
            .FirstOrDefaultAsync();

    public Task InsertAsync(Album album) => _db.InsertAsync(album);

    public Task UpdateAsync(Album album) => _db.UpdateAsync(album);
}