using System.Collections.ObjectModel;

namespace Stickr.ViewModels.Elements;

public class AlbumPageViewModel
{
    public int PageNumber { get; }
    public ObservableCollection<StickerViewModel> Stickers { get; }

    public AlbumPageViewModel(
        int pageNumber,
        IEnumerable<StickerViewModel> stickers)
    {
        PageNumber = pageNumber;
        Stickers = new ObservableCollection<StickerViewModel>(stickers);
    }
}