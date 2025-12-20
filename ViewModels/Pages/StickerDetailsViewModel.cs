using System.Runtime.InteropServices.JavaScript;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

[QueryProperty(nameof(Number), "stickerNumber")]
[QueryProperty(nameof(AlbumId), "albumId")]
public partial class StickerDetailsViewModel : BaseModalPageViewModel, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("albumId", out var value))
        {
            AlbumId = value.ToString() ?? string.Empty;
        }
        
        if (query.TryGetValue("stickerNumber", out var value2))
        {
            Number = Int32.Parse(value2.ToString() ?? string.Empty);
        }
    }
    
    private readonly StickersRepository _stickersRepository;
    private readonly AlbumsRepository _albumsRepository;

    [ObservableProperty]
    private int number;

    [ObservableProperty]
    private string albumTitle = string.Empty;
    
    [ObservableProperty]
    private string albumId = string.Empty;

    [ObservableProperty]
    private int duplicatesCount;

    public bool CanDelete => DuplicatesCount > 1;

    public StickerDetailsViewModel(
        StickersRepository stickersRepository,
        AlbumsRepository albumsRepository)
    {
        _stickersRepository = stickersRepository;
        _albumsRepository = albumsRepository;
    }

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;

        // get by albu and number
        var stickersList = await _stickersRepository.GetByAlbumAndNumberAsync(AlbumId, Number);
        var sticker = stickersList.FirstOrDefault();
        if (sticker == null)
            return;
        
        var album = await _albumsRepository.GetByCollectionIdAsync(sticker.AlbumId);
        AlbumTitle = album?.Title ?? string.Empty;

        DuplicatesCount =
            await _stickersRepository.CountAsync(sticker.AlbumId, sticker.Number);

        OnPropertyChanged(nameof(CanDelete));

        IsBusy = false;
    }

    // ➕ Add duplicate
    [RelayCommand]
    private async Task AddStickerAsync()
    {
        await _stickersRepository.InsertAsync(new Sticker
        {
            AlbumId = AlbumId,
            Number = Number
        });

        DuplicatesCount++;
        OnPropertyChanged(nameof(CanDelete));
    }

    // ➖ Delete (only duplicates) 
    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task DeleteAsync()
    {
        if (!CanDelete)
            return;

        await _stickersRepository.DeleteOneDuplicateAsync(AlbumId, Number);

        DuplicatesCount--;
        OnPropertyChanged(nameof(CanDelete));
    }
}