using Stickr.Models;
using Stickr.Services.Repositories;

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
                //Pages = new List<Page> { new Page { Number = 1, FirstSticker = 1, LastSticker = 12 } }
            };
            
            await _collectionsRepository.InsertAsync(defaultCollection);
        }
    }
}