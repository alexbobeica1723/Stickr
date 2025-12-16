using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

public partial class AlbumDetailsViewModel : BaseModalPageViewModel
{
    private readonly AlbumsRepository _albumsRepository;
    private readonly CollectionsRepository _collectionsRepository;

    public AlbumDetailsViewModel(
        AlbumsRepository albumsRepository,
        CollectionsRepository collectionsRepository)
    {
        _albumsRepository = albumsRepository;
        _collectionsRepository = collectionsRepository;
    }

    private string _albumId = string.Empty;

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public void SetAlbumId(string albumId)
    {
        _albumId = albumId;
    }

    public override async Task InitializeDataAsync()
    {
        if (string.IsNullOrWhiteSpace(_albumId))
            return;

        IsBusy = true;

        var album = await _albumsRepository.GetByCollectionIdAsync(_albumId);
        if (album == null)
            return;

        Title = album.Title;
        OnPropertyChanged(nameof(Title));

        // Load description from Collection (single source of truth)
        var collection =
            await _collectionsRepository.GetByIdAsync(album.CollectionId);

        Description = collection?.Description ?? string.Empty;
        OnPropertyChanged(nameof(Description));

        IsBusy = false;
    }
}