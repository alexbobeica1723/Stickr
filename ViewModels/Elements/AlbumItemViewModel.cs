using System.Windows.Input;
using Stickr.Models;
using Stickr.ViewModels.Base;
using Stickr.Views.Pages;

namespace Stickr.ViewModels.Elements;

public class AlbumItemViewModel : BaseViewModel
{
    public Album Album { get; }

    public string Title => Album.Title;
    public int TotalStickers => Album.TotalStickers;

    public ICommand OpenDetailsCommand { get; }

    public AlbumItemViewModel(Album album)
    {
        Album = album;

        OpenDetailsCommand = new Command(OnOpenDetails);
    }

    private async void OnOpenDetails()
    {
        await Shell.Current.GoToAsync(
            $"{nameof(AlbumDetailsView)}?albumId={Album.CollectionId}");
    }
}