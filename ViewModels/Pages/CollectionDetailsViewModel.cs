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

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(CollectionId), NavigationParameters.CollectionId)]
public partial class CollectionDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    #region Fields
    
    private string _stickerPattern;
    private bool _hasCameraPermission;
    
    #endregion
    
    #region Properties
    
    [ObservableProperty] private string _collectionId;
    public ObservableCollection<CollectionPageViewModel> Pages { get; } = [];
    [ObservableProperty] private string _imagePath;
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _description;
    [ObservableProperty]
    private ObservableCollection<Sticker> stickers = new();
    
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task ScanStickersAsync()
    {
        if (string.IsNullOrWhiteSpace(_collectionId))
        {
            return;
        }

        var detectedNumbers = new List<int>();

        try
        {
            if (!_hasCameraPermission)
            {
                var status = await _permissionsService.RequestCameraPermissionAsync();
                
                if (status != PermissionStatus.Granted)
                {
                    return;
                }
                
                _hasCameraPermission = true;
            }

            var pickResult = await MediaPicker.CapturePhotoAsync();
            if (pickResult is null)
            {
                return;  
            }

            await using var imageAsStream = await pickResult.OpenReadAsync();
            var imageAsBytes = new byte[imageAsStream.Length];
            await imageAsStream.ReadAsync(imageAsBytes);

            var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

            if (!ocrResult.Success)
            {
                return;
            }

            var codes = ExtractValidStickerCodes(ocrResult.Lines);

            foreach (var code in codes)
            {
                if (int.TryParse(code, out var number))
                {
                    detectedNumbers.Add(number); 
                }
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
        {
            return;
        }
        
        var newStickers = detectedNumbers
            .Select(n => new Sticker
            {
                CollectionId = CollectionId,
                Number = n
            })
            .ToList();

        await _stickersRepository.InsertMultipleStickersAsync(newStickers);
        
        foreach (var sticker in newStickers)Stickers.Add(sticker);
        var collection = await _collectionsRepository.GetCollectionByIdAsync(CollectionId);
        
        RebuildPages(collection);
    }
    
    [RelayCommand]
    private async Task OpenStatsAsync()
    {
        await _navigationService.NavigateWithOneParameterAsync(
            NavigationRoutes.CollectionStatsPage,
            NavigationParameters.CollectionId,
            _collectionId);
    }
    
    #endregion
    
    #region Constructors & Dependencies
    
    private readonly IDisplayAlertService _displayAlertService;
    private readonly INavigationService _navigationService;
    private readonly IPermissionsService _permissionsService;
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IStickersRepository _stickersRepository;

    public CollectionDetailsViewModel(
        IDisplayAlertService displayAlertService,
        INavigationService navigationService,
        IPermissionsService permissionsService,
        ICollectionsRepository collectionsRepository,
        IStickersRepository stickersRepository)
    {
        _displayAlertService = displayAlertService;
        _navigationService = navigationService;
        _permissionsService = permissionsService;
        _collectionsRepository = collectionsRepository;
        _stickersRepository = stickersRepository;
    }
    
    #endregion
    
    #region Public Methods
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(NavigationParameters.CollectionId, out var value))
        {
            CollectionId = value.ToString() ?? string.Empty;
        }
    }

    public override async Task InitializeDataAsync()
    {
        if (string.IsNullOrWhiteSpace(CollectionId))
        {
            return; 
        }

        IsBusy = true;
        
        var collection = await _collectionsRepository.GetCollectionByIdAsync(CollectionId);

        if (collection is null)
        {
            return;
        }

        Title = collection.Title;
        ImagePath = collection.Image;
        _stickerPattern = collection.StickerRegexPattern;

        await UpdateStickersAsync();
        
        Pages.Clear();
        RebuildPages(collection);

        IsBusy = false;
    }
    
    #endregion
    
    #region Private Methods
    
    private async Task UpdateStickersAsync()
    {
        var list = await _stickersRepository.GetStickersByCollectionIdAsync(_collectionId);
        Stickers = new ObservableCollection<Sticker>(list);
    }
    
    private async Task<bool> CheckCameraPermission()
    {
        var status = await _permissionsService.CheckCameraPermissionAsync();
        
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
    
    private void RebuildPages(Collection? album)
    {
        if (album is null)
        {
            return;
        }
        
        Pages.Clear();
        
        var collectedStickerCounts = Stickers
            .GroupBy(s => s.Number)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var page in album.Pages.OrderBy(p => p.Number))
        {
            var stickerViewModels = new List<StickerViewModel>();

            for (var stickerNumber = page.FirstSticker;
                 stickerNumber <= page.LastSticker;
                 stickerNumber++)
            {
                var isCollected =
                    collectedStickerCounts.TryGetValue(stickerNumber, out var count)
                    && count > 0;

                stickerViewModels.Add(
                    new StickerViewModel(
                        _navigationService,
                        _collectionId,
                        stickerNumber,
                        isCollected: isCollected
                    )
                );
            }

            Pages.Add(
                new CollectionPageViewModel(
                    pageNumber: page.Number,
                    stickers: stickerViewModels
                )
            );
        }
    }
    
    #endregion
}