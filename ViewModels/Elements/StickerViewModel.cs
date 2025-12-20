using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.ViewModels.Base;
using Stickr.Views.Pages;

namespace Stickr.ViewModels.Elements;

public partial class StickerViewModel : BaseViewModel
{
    public int StickerNumber { get; }
    private string AlbumId { get; }

    [ObservableProperty]
    private bool isCollected;

    [RelayCommand]
    private async Task NavigateToDetails()
    {
        await Shell.Current.GoToAsync(
            nameof(StickerDetailsView),
            new Dictionary<string, object>
            {
                ["stickerNumber"] = StickerNumber,
                ["albumId"] = AlbumId
            });
    }

    public StickerViewModel(string albumId, int stickerNumber, bool isCollected)
    {
        AlbumId = albumId;
        StickerNumber = stickerNumber;
        IsCollected = isCollected;
    }
}