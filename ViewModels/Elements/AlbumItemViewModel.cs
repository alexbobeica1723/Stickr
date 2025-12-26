using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class AlbumItemViewModel : BaseViewModel
{
    #region Properties

    private Album Album { get; }
    public string Title => Album.Title;
    public int TotalStickers => Album.TotalStickers;
    
    #endregion
    
    #region Commands

    [RelayCommand]
    private async Task OpenAlbumDetailsAsync()
    {
        await _navigationService.NavigateWithOneParameterAsync(NavigationRoutes.AlbumDetailsPage,
            NavigationParameters.AlbumId, Album.CollectionId);
    }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly INavigationService _navigationService;
    
    public AlbumItemViewModel(INavigationService navigationService, Album album)
    {
        _navigationService = navigationService;
        Album = album;
    }
    
    #endregion
}