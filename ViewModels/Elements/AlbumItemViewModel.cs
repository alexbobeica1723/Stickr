using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class AlbumItemViewModel : BaseViewModel
{
    #region Properties

    private Album Album { get; }
    public string Title => Album.Title;
    public string Image => Album.Image;
    public int TotalStickers => Album.TotalStickers;
    [ObservableProperty]
    private double _progress;
    
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
    private readonly IStickersRepository _stickersRepository;
    
    public AlbumItemViewModel(INavigationService navigationService,
        Album album,
        IStickersRepository stickerRepository)
    {
        _navigationService = navigationService;
        _stickersRepository = stickerRepository;
        Album = album;
        SetProgress();
    }
    
    #endregion
    
    #region Private Methods

    private async Task SetProgress()
    {
        var uniqueStickers = await _stickersRepository.GetUniqueStickerCountAsync(Album.CollectionId);
        
        Progress = uniqueStickers /  (double)Album.TotalStickers;
    }
    
    #endregion
}