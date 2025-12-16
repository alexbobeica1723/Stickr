using SQLite;
using Stickr.Models;

namespace Stickr.Services.Implementations;

public class DatabaseInitializer
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseInitializer(SQLiteAsyncConnection db)
    {
        _db = db;
    }

    public async Task InitializeAsync()
    {
        await _db.CreateTableAsync<Collection>();
        await _db.CreateTableAsync<Album>();
    }
}