using System.Text.Json;
using SQLite;

namespace Stickr.Models;

[Table("Collections")]
public class Collection
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Image representing the album (cover)
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// Total number of stickers in the album
    /// </summary>
    public int TotalStickers { get; set; }
    
    // Stored as JSON for now
    public string PagesJson { get; set; } = string.Empty;

    [Ignore]
    public List<Page> Pages
    {
        get =>
            string.IsNullOrWhiteSpace(PagesJson)
                ? new List<Page>()
                : JsonSerializer.Deserialize<List<Page>>(PagesJson)!;

        set =>
            PagesJson = JsonSerializer.Serialize(value);
    }
}