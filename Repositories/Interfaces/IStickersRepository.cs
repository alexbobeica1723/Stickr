using Stickr.Models;

namespace Stickr.Repositories.Interfaces;

public interface IStickersRepository
{
    Task<List<Sticker>> GetStickersByCollectionIdAsync(string collectionId);
    Task InsertStickerAsync(Sticker sticker);
    Task InsertMultipleStickersAsync(IEnumerable<Sticker> stickers);
    Task<int> CountCollectionStickersAsync(string collectionId, int number);
    Task<List<Sticker>> GetStickersByCollectionAndNumberAsync(string collectionId, int number);
    Task<int> GetUniqueStickerCountAsync(string collectionId);
    Task<bool> DeleteDuplicatedStickerAsync(string collectionId, int number);
}