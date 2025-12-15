using SQLite;
using Stickr.Models;

namespace Stickr.Services.Implementations;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
    }

    public async Task InitializeAsync()
    {
        await _db.CreateTableAsync<Collection>();
    }

    // Collections
    public Task<List<Collection>> GetCollectionsAsync() =>
        _db.Table<Collection>().ToListAsync();

    public Task<Collection?> GetCollectionAsync(string id) =>
        _db.Table<Collection>()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

    public Task<int> SaveCollectionAsync(Collection collection) =>
        _db.InsertOrReplaceAsync(collection);

    /*public Task<int> DeleteCollectionAsync(Collection collection) =>
        _db.DeleteAsync(collection);*/
}