using SQLite;

namespace Stickr.Models;

public class Sticker
{
    [PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// Album this sticker belongs to
    /// </summary>
    [Indexed]
    public string CollectionId { get; set; } = string.Empty;

    /// <summary>
    /// Sticker number printed on the sticker (e.g. 3, 5, 7)
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// When this sticker was added (useful later)
    /// </summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}