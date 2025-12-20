using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using Page = Stickr.Models.Page;

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(AlbumId), "albumId")]
public partial class AlbumStatsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    private readonly StickersRepository _stickersRepository;
    private readonly AlbumsRepository _albumsRepository;

    [ObservableProperty]
    private string _albumId;

    public ObservableCollection<int> MissingStickers { get; } = new();
    public ObservableCollection<DuplicatedStickerItem> DuplicatedStickers { get; } = new();

    public AlbumStatsViewModel(AlbumsRepository albumsRepository,
        StickersRepository stickersRepository)
    {
        _albumsRepository = albumsRepository;
        _stickersRepository = stickersRepository;
    }

    public override async Task InitializeDataAsync()
    {
        var stickers = await _stickersRepository.GetByAlbumIdAsync(AlbumId);

        await BuildMissingStickers(stickers);
        BuildDuplicatedStickers(stickers);
    }

    private async Task BuildMissingStickers(IEnumerable<Sticker> stickers)
    {
        MissingStickers.Clear();

        var album = await _albumsRepository.GetByCollectionIdAsync(AlbumId);
        if (album == null) return;

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

        foreach (var g in duplicates)
            DuplicatedStickers.Add(
                new DuplicatedStickerItem(g.Key, g.Count()));
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("albumId", out var value))
        {
            AlbumId = value.ToString() ?? string.Empty;
        }
    }
    
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
}