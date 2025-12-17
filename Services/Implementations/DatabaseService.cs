using Stickr.Models;
using SQLite;

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
        await _db.CreateTableAsync<Album>();
        await _db.CreateTableAsync<Sticker>(); 
    }

    public SQLiteAsyncConnection GetConnection() => _db;
}