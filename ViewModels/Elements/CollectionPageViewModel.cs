using System.Collections.ObjectModel;

namespace Stickr.ViewModels.Elements;

public class CollectionPageViewModel
{
    #region Properties
    
    public int PageNumber { get; }
    public ObservableCollection<StickerViewModel> Stickers { get; }
    
    #endregion

    #region Constructor & Dependencies

    public CollectionPageViewModel(
        int pageNumber,
        IEnumerable<StickerViewModel> stickers)
    {
        PageNumber = pageNumber;
        Stickers = new ObservableCollection<StickerViewModel>(stickers);
    }
    
    #endregion
}