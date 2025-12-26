using Stickr.Models;
using SQLite;
using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class DatabaseService : IDatabaseService
{
    private readonly SQLiteAsyncConnection _db;

    public DatabaseService()
    {
        _db = new SQLiteAsyncConnection("stickr.db3");
    }

    public async Task InitializeAsync()
    {
        await _db.CreateTableAsync<Collection>();
        await _db.CreateTableAsync<Album>();
        await _db.CreateTableAsync<Sticker>(); 
    }

    public SQLiteAsyncConnection GetConnection() => _db;
}