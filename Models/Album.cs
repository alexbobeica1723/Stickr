using System.Text.Json;
using SQLite;

namespace Stickr.Models;

[Table("Albums")]
public class Album
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Indexed]
    public string CollectionId { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;

    public int TotalStickers { get; set; }
    
    /// <summary>
    /// Regex pattern used to extract sticker codes from OCR text
    /// Examples:
    ///  - @"\b\d{1,3}\b"
    ///  - @"\b(GER|AUS)\s?\d{1,2}\b"
    /// </summary>
    public string StickerRegexPattern { get; set; } = string.Empty;

    /// <summary>
    /// How many stickers the user owns so far
    /// </summary>
    public int CollectedStickers { get; set; } = 0;

    public string PagesJson { get; set; } = "[]";
    
    [Ignore]
    public IReadOnlyList<Page> Pages
    {
        get => JsonSerializer.Deserialize<List<Page>>(PagesJson) ?? [];
        set => PagesJson = JsonSerializer.Serialize(value);
    }
}