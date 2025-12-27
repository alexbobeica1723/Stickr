using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(Number), NavigationParameters.StickerNumber)]
[QueryProperty(nameof(AlbumId), NavigationParameters.AlbumId)]
public partial class StickerDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    #region Properties
    
    [ObservableProperty]
    private int number;
    [ObservableProperty]
    private string albumTitle = string.Empty;
    [ObservableProperty]
    private string albumId = string.Empty;
    [ObservableProperty]
    private int duplicatesCount;
    public bool CanDelete => DuplicatesCount > 1;
    
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task AddStickerAsync()
    {
        await _stickersRepository.InsertStickerAsync(new Sticker
        {
            AlbumId = AlbumId,
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
        
        await _stickersRepository.DeleteDuplicatedStickerAsync(AlbumId, Number);

        DuplicatesCount--;
        OnPropertyChanged(nameof(CanDelete));
    }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly IStickersRepository _stickersRepository;
    private readonly IAlbumsRepository _albumsRepository;

    public StickerDetailsViewModel(
        IStickersRepository stickersRepository,
        IAlbumsRepository albumsRepository)
    {
        _stickersRepository = stickersRepository;
        _albumsRepository = albumsRepository;
    }
    
    #endregion
    
    #region Public Methods

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;
        
        var stickersList = await _stickersRepository.GetStickersByAlbumAndNumberAsync(AlbumId, Number);
        var sticker = stickersList.FirstOrDefault();
        if (sticker is null)
        {
            return;       
        }
        
        var album = await _albumsRepository.GetAlbumByCollectionIdAsync(sticker.AlbumId);
        AlbumTitle = album.Title;

        DuplicatesCount =
            await _stickersRepository.CountStickersAsync(sticker.AlbumId, sticker.Number);

        OnPropertyChanged(nameof(CanDelete));

        IsBusy = false;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(NavigationParameters.AlbumId, out var value))
        {
            AlbumId = value.ToString() ?? string.Empty;
        }
        
        if (query.TryGetValue(NavigationParameters.StickerNumber, out var value2))
        {
            Number = Int32.Parse(value2.ToString() ?? string.Empty);
        }
    }
    
    #endregion
}