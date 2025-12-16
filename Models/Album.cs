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
    /// How many stickers the user owns so far
    /// </summary>
    public int CollectedStickers { get; set; } = 0;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
}