using System.Text.Json;
using SQLite;

namespace Stickr.Models;

[Table("Albums")]
public class Album
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string CollectionId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public int TotalStickers { get; set; }

    /// <summary>
    /// Stickers the user already owns
    /// </summary>
    public string OwnedStickersSerialized { get; set; } = string.Empty;
    
    /*public List<int> OwnedStickers
    {
        get =>
            string.IsNullOrWhiteSpace(OwnedStickersSerialized)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(OwnedStickersSerialized)!;

        set =>
            OwnedStickersSerialized = JsonSerializer.Serialize(value);
    }*/
}