using Stickr.Models;
using SQLite;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;

namespace Stickr.Repositories.Implementations;

public class CollectionsRepository : ICollectionsRepository
{
    private readonly SQLiteAsyncConnection _db;

    public CollectionsRepository(IDatabaseService dbService)
    {
        _db = dbService.GetConnection();
    }

    public Task<List<Collection>> GetCollectionsAsync() => _db.Table<Collection>().ToListAsync();
    public Task InsertCollectionAsync(Collection collection) => _db.InsertAsync(collection);
    public Task<int> UpdateCollectionAsync(Collection collection) => _db.InsertOrReplaceAsync(collection);
}