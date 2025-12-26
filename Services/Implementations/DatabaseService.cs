using Stickr.Models;
using SQLite;
using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class DatabaseService : IDatabaseService
{
    #region Fields

    private const string DatabaseName = "stickr.db3";
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly SQLiteAsyncConnection _databaseConnection;

    public DatabaseService()
    {
        _databaseConnection = new SQLiteAsyncConnection(DatabaseName);
    }
    
    #endregion

    #region Public Methods

    public async Task InitializeAsync()
    {
        await _databaseConnection.CreateTableAsync<Collection>();
        await _databaseConnection.CreateTableAsync<Album>();
        await _databaseConnection.CreateTableAsync<Sticker>(); 
    }

    public SQLiteAsyncConnection GetConnection() => _databaseConnection;
    
    #endregion
}