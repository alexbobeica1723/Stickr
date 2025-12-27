using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(AlbumId), NavigationParameters.AlbumId)]
public partial class AlbumStatsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    #region Properties

    [ObservableProperty]
    private string _albumId;
    public ObservableCollection<int> MissingStickers { get; } = new();
    public ObservableCollection<DuplicatedStickerItem> DuplicatedStickers { get; } = new();
    
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task CopyMissingAsync()
    {
        var text = string.Join(", ", MissingStickers);
        
        await Clipboard.Default.SetTextAsync(text);
    }

    [RelayCommand]
    private async Task CopyDuplicatesAsync()
    {
        var text = string.Join(", ",
            DuplicatedStickers.Select(d => $"{d.Number} (x{d.Count})"));

        await Clipboard.Default.SetTextAsync(text);
    }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly IStickersRepository _stickersRepository;
    private readonly IAlbumsRepository _albumsRepository;

    public AlbumStatsViewModel(IAlbumsRepository albumsRepository,
        IStickersRepository stickersRepository)
    {
        _albumsRepository = albumsRepository;
        _stickersRepository = stickersRepository;
    }
    
    #endregion
    
    #region Public Methods

    public override async Task InitializeDataAsync()
    {
        var stickers = await _stickersRepository.GetStickersByAlbumIdAsync(AlbumId);

        await BuildMissingStickers(stickers);
        BuildDuplicatedStickers(stickers);
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(NavigationParameters.AlbumId, out var value))
        {
            AlbumId = value.ToString() ?? string.Empty;
        }
    }
    
    #endregion
    
    #region Private Methods

    private async Task BuildMissingStickers(IEnumerable<Sticker> stickers)
    {
        MissingStickers.Clear();

        var album = await _albumsRepository.GetAlbumByCollectionIdAsync(AlbumId);

        var expected = album.Pages
            .SelectMany(p => Enumerable.Range(
                p.FirstSticker,
                p.LastSticker - p.FirstSticker + 1))
            .ToHashSet();

        var owned = stickers
            .Select(s => s.Number)
            .ToHashSet();

        foreach (var number in expected.Except(owned).OrderBy(n => n))
            MissingStickers.Add(number);
    }

    private void BuildDuplicatedStickers(IEnumerable<Sticker> stickers)
    {
        DuplicatedStickers.Clear();

        var duplicates = stickers
            .GroupBy(s => s.Number)
            .Where(g => g.Count() > 1)
            .OrderBy(g => g.Key);

        // Subtract 1 at the end because we consider one sticker as already added to the album
        foreach (var g in duplicates)
            DuplicatedStickers.Add(
                new DuplicatedStickerItem(g.Key, g.Count() - 1));
    }
    
    #endregion
}