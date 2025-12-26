using SQLite;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class AlbumsRepository : IAlbumsRepository
{
    #region Constructor & Dependencies
    
    private readonly SQLiteAsyncConnection _databaseConnection;
    
    public AlbumsRepository(IDatabaseService dbService)
    {
        _databaseConnection = dbService.GetConnection();
    }
    
    #endregion

    #region Public Methods

    public Task<List<Album>> GetAlbumsAsync() => _databaseConnection.Table<Album>().ToListAsync();

    public Task<Album> GetAlbumByCollectionIdAsync(string collectionId)
        => _databaseConnection.Table<Album>()
            .Where(a => a.CollectionId == collectionId).FirstOrDefaultAsync();

    public Task InsertAlbumAsync(Album album) => _databaseConnection.InsertAsync(album);
    
    #endregion
}