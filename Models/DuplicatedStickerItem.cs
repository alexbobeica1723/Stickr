namespace Stickr.Models;

public class DuplicatedStickerItem
{
    public int Number { get; }
    public int Count { get; }

    public DuplicatedStickerItem(int number, int count)
    {
        Number = number;
        Count = count;
    }
}