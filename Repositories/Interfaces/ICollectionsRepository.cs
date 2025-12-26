using Stickr.Models;

namespace Stickr.Repositories.Interfaces;

public interface ICollectionsRepository
{
    Task<List<Collection>> GetCollectionsAsync();
    Task InsertCollectionAsync(Collection collection);
    Task<int> UpdateCollectionAsync(Collection collection);
}