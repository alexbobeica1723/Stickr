using Stickr.Models;
using SQLite;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class CollectionsRepository : ICollectionsRepository
{
    #region Constructor & Dependencies
    
    private readonly SQLiteAsyncConnection _databaseConnection;

    public CollectionsRepository(IDatabaseService dbService)
    {
        _databaseConnection = dbService.GetConnection();
    }
    
    #endregion

    #region Public Methods
    
    public Task<List<Collection>> GetCollectionsAsync() => _databaseConnection.Table<Collection>().ToListAsync();
    public Task<Collection> GetCollectionByIdAsync(string collectionId)
        => _databaseConnection.Table<Collection>()
            .Where(c => c.Id == collectionId).FirstOrDefaultAsync();
    public Task InsertCollectionAsync(Collection collection) => _databaseConnection.InsertAsync(collection);
    public Task<int> UpdateCollectionAsync(Collection collection) => _databaseConnection.InsertOrReplaceAsync(collection);
    
    #endregion
}