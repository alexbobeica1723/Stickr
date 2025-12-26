using Stickr.Models;

namespace Stickr.Repositories.Interfaces;

public interface IAlbumsRepository
{
    Task<List<Album>> GetAlbumsAsync();
    Task<Album> GetAlbumByCollectionIdAsync(string collectionId);
    Task InsertAlbumAsync(Album album);
}