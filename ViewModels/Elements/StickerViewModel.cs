using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class StickerViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public int StickerNumber { get; }
    private string AlbumId { get; }

    [ObservableProperty]
    private bool isCollected;

    [RelayCommand]
    private async Task NavigateToDetails()
    {
        await _navigationService.NavigateWithMultipleParametersAsync(
            NavigationRoutes.StickerDetailsPage,
            new Dictionary<string, object>
            {
                [NavigationParameters.StickerNumber] = StickerNumber,
                [NavigationParameters.AlbumId] = AlbumId
            });
    }

    public StickerViewModel(INavigationService navigationService, 
        string albumId,
        int stickerNumber, 
        bool isCollected)
    {
        _navigationService = navigationService;
        AlbumId = albumId;
        StickerNumber = stickerNumber;
        IsCollected = isCollected;
    }
}