using Stickr.Models;
using SQLite;
using Stickr.Services.Implementations;

namespace Stickr.Services.Repositories;

public class CollectionsRepository
{
    private readonly SQLiteAsyncConnection _db;

    public CollectionsRepository(DatabaseService dbService)
    {
        _db = dbService.GetConnection();
    }

    public Task<List<Collection>> GetAllAsync() => _db.Table<Collection>().ToListAsync();
    public Task InsertAsync(Collection collection) => _db.InsertAsync(collection);
    public Task<int> UpdateAsync(Collection collection) => _db.InsertOrReplaceAsync(collection);
    
    public Task<Collection?> GetByIdAsync(string id) =>
        _db.Table<Collection>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
}