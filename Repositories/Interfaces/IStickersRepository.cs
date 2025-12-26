using Stickr.Models;

namespace Stickr.Repositories.Interfaces;

public interface IStickersRepository
{
    Task<List<Sticker>> GetStickersByAlbumIdAsync(string albumId);
    Task InsertStickerAsync(Sticker sticker);
    Task InsertMultipleStickersAsync(IEnumerable<Sticker> stickers);
    Task<int> CountStickersAsync(string albumId, int number);
    Task<List<Sticker>> GetStickersByAlbumAndNumberAsync(string albumId, int number);
    Task<bool> DeleteDuplicateStickerAsync(string albumId, int number);
}