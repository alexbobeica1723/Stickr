using Stickr.Models;
using Page = Stickr.Models.Page;

namespace Stickr.Services.Implementations;

public class SeedService
{
    private readonly DatabaseService _db;

    public SeedService(DatabaseService db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        var existing = await _db.GetCollectionsAsync();
        if (existing.Any())
            return;

        var collection = new Collection
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Superliga România 2025",
            Description = "Album oficial Panini Superliga",
            Image = "",
            TotalStickers = 300,
            Pages =
            {
                new Page { Number = 1, FirstSticker = 1, LastSticker = 13 },
                new Page { Number = 2, FirstSticker = 14, LastSticker = 21 },
                new Page { Number = 3, FirstSticker = 22, LastSticker = 30 },
            }
        };

        await _db.SaveCollectionAsync(collection);
    }
}