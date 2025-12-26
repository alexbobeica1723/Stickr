using System.Collections.ObjectModel;

namespace Stickr.ViewModels.Elements;

public class AlbumPageViewModel
{
    #region Properties
    
    public int PageNumber { get; }
    public ObservableCollection<StickerViewModel> Stickers { get; }
    
    #endregion

    #region Constructor & Dependencies

    public AlbumPageViewModel(
        int pageNumber,
        IEnumerable<StickerViewModel> stickers)
    {
        PageNumber = pageNumber;
        Stickers = new ObservableCollection<StickerViewModel>(stickers);
    }
    
    #endregion
}