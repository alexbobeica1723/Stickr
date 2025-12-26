using SQLite;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class AlbumsRepository : IAlbumsRepository
{
    private readonly SQLiteAsyncConnection _db;

    public AlbumsRepository(IDatabaseService dbService)
    {
        _db = dbService.GetConnection();
    }

    public Task<List<Album>> GetAlbumsAsync() => _db.Table<Album>().ToListAsync();

    public Task<Album?> GetAlbumByCollectionIdAsync(string collectionId)
        => _db.Table<Album>()
            .Where(a => a.CollectionId == collectionId)
            .FirstOrDefaultAsync();

    public Task InsertAlbumAsync(Album album) => _db.InsertAsync(album);
}