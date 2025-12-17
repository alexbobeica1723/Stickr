using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
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
    private readonly StickersRepository _stickersRepository;
    
    [RelayCommand]
    private async Task AddTestStickersAsync()
    {
        if (string.IsNullOrWhiteSpace(_albumId))
            return;

        var newStickers = new[]
        {
            new Sticker { AlbumId = _albumId, Number = 3 },
            new Sticker { AlbumId = _albumId, Number = 5 },
            new Sticker { AlbumId = _albumId, Number = 7 }
        };

        await _stickersRepository.InsertManyAsync(newStickers);

        // Update UI immediately
        foreach (var sticker in newStickers)
            Stickers.Add(sticker);
    }

    public AlbumDetailsViewModel(
        AlbumsRepository albumsRepository,
        CollectionsRepository collectionsRepository,
        StickersRepository stickersRepository)
    {
        _albumsRepository = albumsRepository;
        _collectionsRepository = collectionsRepository;
        _stickersRepository = stickersRepository;
    }

    private string _albumId = string.Empty;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _description;
    
    [ObservableProperty]
    private IReadOnlyList<Stickr.Models.Page> _pages;

    [ObservableProperty]
    private ObservableCollection<Sticker> stickers;

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
        Description = "Test";
        
        var loadedStickers = await _stickersRepository.GetByAlbumIdAsync(_albumId);
        Stickers = new ObservableCollection<Sticker>(loadedStickers);

        IsBusy = false;
    }
}