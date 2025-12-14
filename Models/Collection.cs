namespace Stickr.Models;

public class Collection
{
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
}