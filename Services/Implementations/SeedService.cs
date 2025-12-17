using Stickr.Models;
using Stickr.Services.Repositories;
using Page = Stickr.Models.Page;

namespace Stickr.Services.Implementations;

public class SeedService
{
    private readonly CollectionsRepository _collectionsRepository;

    public SeedService(CollectionsRepository collectionsRepository)
    {
        _collectionsRepository = collectionsRepository;
    }

    public async Task SeedAsync()
    {
        var existing = await _collectionsRepository.GetAllAsync();
        if (!existing.Any())
        {
            var defaultCollection = new Collection
            {
                Title = "My First Album",
                Description = "Starter album",
                Image = "album.png",
                TotalStickers = 100,
                StickerRegexPattern = @"^\d{1,3}$",
                Pages = new List<Page>
                {
                    new() { Number = 1, FirstSticker = 1, LastSticker = 13 },
                    new() { Number = 2, FirstSticker = 14, LastSticker = 21 },
                    new() { Number = 3, FirstSticker = 22, LastSticker = 30 }
                }
            };
            
            await _collectionsRepository.InsertAsync(defaultCollection);
        }
    }
}