using System.Threading.Tasks;
using Stickr.Models;
using Stickr.Services.Repositories;
using Page = Stickr.Models.Page;

namespace Stickr.Services.Implementations;

public class SeedService
{
    private readonly CollectionsRepository _collectionsRepo;

    public SeedService(CollectionsRepository collectionsRepo)
    {
        _collectionsRepo = collectionsRepo;
    }

    public async Task SeedAsync()
    {
        var existing = await _collectionsRepo.GetAllAsync();
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
            await _collectionsRepo.InsertAsync(defaultCollection);
        }
    }
}