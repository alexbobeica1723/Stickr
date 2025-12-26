using System.Windows.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public class AlbumItemViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    public Album Album { get; }

    public string Title => Album.Title;
    public int TotalStickers => Album.TotalStickers;

    public ICommand OpenDetailsCommand { get; }

    public AlbumItemViewModel(INavigationService navigationService, Album album)
    {
        _navigationService = navigationService;
        Album = album;

        OpenDetailsCommand = new Command(OnOpenDetails);
    }

    private async void OnOpenDetails()
    {
        await _navigationService.NavigateWithOneParameterAsync(NavigationRoutes.AlbumDetailsPage,
            NavigationParameters.AlbumId, Album.CollectionId);
    }
}