using CommunityToolkit.Mvvm.ComponentModel;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

public partial class AlbumDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("albumId", out var value))
        {
            _albumId = value.ToString() ?? string.Empty;
        }
    }
    
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

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _description;
    
    [ObservableProperty]
    private IReadOnlyList<Stickr.Models.Page> _pages;

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
        Pages = album.Pages;

        // Load description from Collection (single source of truth)
        var collection =
            await _collectionsRepository.GetByIdAsync(album.CollectionId);

        Description = collection?.Description ?? string.Empty;

        IsBusy = false;
    }
}