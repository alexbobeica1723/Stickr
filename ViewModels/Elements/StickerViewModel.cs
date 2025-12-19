using CommunityToolkit.Mvvm.ComponentModel;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class StickerViewModel : BaseViewModel
{
    public int StickerNumber { get; }

    [ObservableProperty]
    private bool isCollected;

    public StickerViewModel(int stickerNumber, bool isCollected)
    {
        StickerNumber = stickerNumber;
        IsCollected = isCollected;
    }
}