using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(Number), NavigationParameters.StickerNumber)]
[QueryProperty(nameof(CollectionId), NavigationParameters.CollectionId)]
public partial class StickerDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    #region Properties
    
    [ObservableProperty]
    private int _number;
    [ObservableProperty]
    private string _collectionTitle = string.Empty;
    [ObservableProperty]
    private string _collectionId = string.Empty;
    [ObservableProperty]
    private int _duplicatesCount;
    public bool CanDelete => DuplicatesCount > 1;
    
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task AddStickerAsync()
    {
        await _stickersRepository.InsertStickerAsync(new Sticker
        {
            CollectionId = CollectionId,
            Number = Number
        });

        DuplicatesCount++;
        OnPropertyChanged(nameof(CanDelete));
    }
    
    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task DeleteAsync()
    {
        if (!CanDelete)
        {
            return;     
        }
        
        await _stickersRepository.DeleteDuplicatedStickerAsync(CollectionId, Number);

        DuplicatesCount--;
        OnPropertyChanged(nameof(CanDelete));
    }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly IStickersRepository _stickersRepository;
    private readonly ICollectionsRepository _collectionsRepository;

    public StickerDetailsViewModel(
        IStickersRepository stickersRepository,
        ICollectionsRepository collectionsRepository)
    {
        _stickersRepository = stickersRepository;
        _collectionsRepository = collectionsRepository;
    }
    
    #endregion
    
    #region Public Methods

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;
        
        var stickersList = await _stickersRepository.GetStickersByCollectionAndNumberAsync(CollectionId, Number);
        var sticker = stickersList.FirstOrDefault();
        if (sticker is null)
        {
            return;       
        }
        
        var collection = await _collectionsRepository.GetCollectionByIdAsync(sticker.CollectionId);
        CollectionTitle = collection.Title;

        DuplicatesCount =
            await _stickersRepository.CountCollectionStickersAsync(sticker.CollectionId, sticker.Number);

        OnPropertyChanged(nameof(CanDelete));

        IsBusy = false;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(NavigationParameters.CollectionId, out var value))
        {
            CollectionId = value.ToString() ?? string.Empty;
        }
        
        if (query.TryGetValue(NavigationParameters.StickerNumber, out var value2))
        {
            Number = Int32.Parse(value2.ToString() ?? string.Empty);
        }
    }
    
    #endregion
}