using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class StickerViewModel : BaseViewModel
{
    #region Properties
    
    public int StickerNumber { get; }
    private string AlbumId { get; }
    [ObservableProperty]
    private bool isCollected;
    
    #endregion

    #region Commands

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
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly INavigationService _navigationService;

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
    
    #endregion
}