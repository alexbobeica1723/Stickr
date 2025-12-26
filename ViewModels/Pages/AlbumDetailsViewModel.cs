using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.OCR;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;
using Stickr.ViewModels.Elements;
using Stickr.Views.Pages;
using Page = Stickr.Models.Page;

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
    
    private readonly IDisplayAlertService _displayAlertService;
    private readonly INavigationService _navigationService;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IStickersRepository _stickersRepository;
    
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

        await _stickersRepository.InsertMultipleStickersAsync(newStickers);

        // Update UI immediately
        foreach (var sticker in newStickers)
            Stickers.Add(sticker);
        
        var album =  await _albumsRepository.GetAlbumByCollectionIdAsync(_albumId);
        RebuildPages(album);
    }
    
    [RelayCommand]
    private async Task ScanStickersAsync()
    {
        if (string.IsNullOrWhiteSpace(_albumId))
            return;

        var detectedNumbers = new List<int>();

        try
        {
            var hasPermission = await CheckCameraPermission();
            if (!hasPermission)
                return;

            var pickResult = await MediaPicker.CapturePhotoAsync();
            if (pickResult == null)
                return;

            await using var imageAsStream = await pickResult.OpenReadAsync();
            var imageAsBytes = new byte[imageAsStream.Length];
            await imageAsStream.ReadAsync(imageAsBytes);

            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

            if (!ocrResult.Success)
                return;

            var codes = ExtractValidStickerCodes(ocrResult.Lines);

            foreach (var code in codes)
            {
                if (int.TryParse(code, out var number))
                    detectedNumbers.Add(number);
            }
        }
        catch
        {
            return;
        }

        if (detectedNumbers.Count == 0)
        {
            await _displayAlertService.DisplayCancelOnlyAlert("No stickers found",
                "No valid sticker numbers were detected.",
                "OK");
            return;
        }

        // 🔹 PREVIEW LIST (sorted, duplicates kept)
        var previewText = string.Join(", ", detectedNumbers.OrderBy(n => n));

        var confirm = await _displayAlertService.DisplayAlert(
            "Add stickers?",
            $"The following stickers will be added:\n\n{previewText}",
            "OK",
            "Cancel");

        if (!confirm)
            return;

        // 🔹 Persist
        var newStickers = detectedNumbers
            .Select(n => new Sticker
            {
                AlbumId = _albumId,
                Number = n
            })
            .ToList();

        await _stickersRepository.InsertMultipleStickersAsync(newStickers);

        // 🔹 Update UI
        foreach (var sticker in newStickers)Stickers.Add(sticker);
        var album = await _albumsRepository.GetAlbumByCollectionIdAsync(_albumId);
        RebuildPages(album);
    }
    
    [RelayCommand]
    private async Task OpenStatsAsync()
    {
        await _navigationService.NavigateWithOneParameterAsync(
            NavigationRoutes.AlbumStatsPage,
            NavigationParameters.AlbumId,
            _albumId);
    }
    
    public ObservableCollection<AlbumPageViewModel> Pages { get; }
        = new();

    [ObservableProperty] private string _imagePath;

    public AlbumDetailsViewModel(
        IDisplayAlertService displayAlertService,
        INavigationService navigationService,
        IAlbumsRepository albumsRepository,
        IStickersRepository stickersRepository)
    {
        _displayAlertService = displayAlertService;
        _navigationService = navigationService;
        _albumsRepository = albumsRepository;
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

        var album = await _albumsRepository.GetAlbumByCollectionIdAsync(_albumId);
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
        var list = await _stickersRepository.GetStickersByAlbumIdAsync(_albumId);
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
                        _navigationService,
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