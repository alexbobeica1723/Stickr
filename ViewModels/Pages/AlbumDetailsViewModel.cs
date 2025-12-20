using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.OCR;
using Stickr.Models;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using Stickr.ViewModels.Elements;

namespace Stickr.ViewModels.Pages;

public partial class AlbumDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    private string _stickerPattern;
    
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
        
        var album =  await _albumsRepository.GetByCollectionIdAsync(_albumId);
        RebuildPages(album);
    }
    
    [RelayCommand]
    private async Task DeleteStickerFiveAsync()
    {
        var deleted = await _stickersRepository
            .DeleteOneDuplicateAsync(_albumId, 5);

        if (!deleted)
        {
            await Shell.Current.DisplayAlert(
                "Not allowed",
                "You can only delete duplicated stickers.",
                "OK");
            return;
        }

        await UpdateStickersAsync();
    }
    
    [RelayCommand]
    private async Task ScanStickersAsync()
    {
        if (string.IsNullOrWhiteSpace(_albumId))
            return;
        
        var foundStickers = new List<string>();
        try
        {
            var hasPermission = await CheckCameraPermission();
            if (!hasPermission)
            {
                return;
            }
            
            var pickResult = await MediaPicker.CapturePhotoAsync();

            if (pickResult != null)
            {
                await using var imageAsStream = await pickResult.OpenReadAsync();
                var imageAsBytes = new byte[imageAsStream.Length];
                await imageAsStream.ReadAsync(imageAsBytes);
                
                var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

                var results = ocrResult.Elements;

                if (ocrResult.Success)
                {
                    foundStickers.AddRange(ExtractValidStickerCodes(ocrResult.Lines));
                }
            }
        }
        catch(Exception ex)
        {
        }
        
        var newStickers = new List<Sticker>();
        foreach (var sticker in foundStickers)
        {
            var stickerNumber = Int32.TryParse(sticker, out var number) ? number : 0;

            if (stickerNumber != 0)
            {
                newStickers.Add(new Sticker { AlbumId = _albumId, Number = stickerNumber });
            }
        }

        await _stickersRepository.InsertManyAsync(newStickers);

        // Update UI immediately
        foreach (var sticker in newStickers)
            Stickers.Add(sticker);
        
        var album = await _albumsRepository.GetByCollectionIdAsync(_albumId);
        RebuildPages(album);
    }
    
    public ObservableCollection<AlbumPageViewModel> Pages { get; }
        = new();

    [ObservableProperty] private string _imagePath;

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
    private ObservableCollection<Sticker> stickers = new();

    public override async Task InitializeDataAsync()
    {
        if (string.IsNullOrWhiteSpace(_albumId))
            return;

        IsBusy = true;

        var album = await _albumsRepository.GetByCollectionIdAsync(_albumId);
        if (album == null)
            return;

        Title = album.Title;
        ImagePath = album.Image;
        _stickerPattern = album.StickerRegexPattern;

        await UpdateStickersAsync();
        
        Pages.Clear();
        RebuildPages(album);

        IsBusy = false;
    }
    
    private async Task UpdateStickersAsync()
    {
        var list = await _stickersRepository.GetByAlbumIdAsync(_albumId);
        Stickers = new ObservableCollection<Sticker>(list);
    }
    
    private async Task<bool> CheckCameraPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }
        return status == PermissionStatus.Granted;
    }
    
    private IEnumerable<string> ExtractValidStickerCodes(IEnumerable<string> ocrLines)
    {
        var regex = new Regex(_stickerPattern, RegexOptions.IgnoreCase);

        foreach (var line in ocrLines)
        {
            var matches = regex.Matches(line);
            foreach (Match match in matches)
            {
                yield return match.Value.Trim();
            }
        }
    }
    
    private void RebuildPages(Album? album)
    {
        if (album == null) return;
        // Clear existing pages so CarouselView refreshes
        Pages.Clear();

        // Build a fast lookup: sticker number -> how many collected
        var collectedStickerCounts = Stickers
            .GroupBy(s => s.Number)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var page in album.Pages.OrderBy(p => p.Number))
        {
            var stickerViewModels = new List<StickerViewModel>();

            for (int stickerNumber = page.FirstSticker;
                 stickerNumber <= page.LastSticker;
                 stickerNumber++)
            {
                var isCollected =
                    collectedStickerCounts.TryGetValue(stickerNumber, out var count)
                    && count > 0;

                stickerViewModels.Add(
                    new StickerViewModel(
                        _albumId,
                        stickerNumber,
                        isCollected: isCollected
                    )
                );
            }

            Pages.Add(
                new AlbumPageViewModel(
                    pageNumber: page.Number,
                    stickers: stickerViewModels
                )
            );
        }
    }
}