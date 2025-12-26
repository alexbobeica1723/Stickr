using SQLite;

namespace Stickr.Services.Interfaces;

public interface IDatabaseService
{
    public Task InitializeAsync();
    public SQLiteAsyncConnection GetConnection();
}