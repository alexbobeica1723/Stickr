using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class CollectionItemViewModel : BaseViewModel
{
    #region Properties
    
    public Collection Model { get; }
    [ObservableProperty]
    private bool _isCollecting;
    [ObservableProperty]
    private bool _isStartCollectionIconVisible;

    public string StatusText =>
        IsCollecting ? "Status: Collecting" : "Status: Available";
    public string TotalStickers => "Stickers to collect: " + Model.TotalStickers;
    
    #endregion

    #region Commands
    
    public IAsyncRelayCommand StartCollectingCommand { get; }
    
    #endregion

    #region Constructor & Dependencies
    
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IDisplayAlertService _displayAlertService;
    
    public CollectionItemViewModel(
        Collection model,
        ICollectionsRepository collectionsRepository,
        IAlbumsRepository albumsRepository,
        IDisplayAlertService displayAlertService)
    {
        Model = model;
        _collectionsRepository =  collectionsRepository;
        _albumsRepository = albumsRepository;
        _displayAlertService = displayAlertService;

        IsCollecting = model.IsCollecting;
        IsStartCollectionIconVisible = !IsCollecting;
        StartCollectingCommand = new AsyncRelayCommand(StartCollectingAsync);
    }
    
    #endregion
    
    #region Private Methods

    partial void OnIsCollectingChanged(bool value)
    {
        OnPropertyChanged(nameof(StatusText));
    }

    private async Task StartCollectingAsync()
    {
        var confirm = await _displayAlertService.DisplayAlert(
            "Start collection?",
            $"Do you wish to start collecting " + Model.Title + "?",
            "Yes",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        IsStartCollectionIconVisible = false;
        
        var album = new Album
        {
            CollectionId = Model.Id,
            Title = Model.Title,
            Image = Model.Image,
            TotalStickers = Model.TotalStickers,
            StickerRegexPattern = Model.StickerRegexPattern,
            Pages = Model.Pages
        };

        await _albumsRepository.InsertAlbumAsync(album);

        Model.IsCollecting = true;
        IsCollecting = true;

        await _collectionsRepository.UpdateCollectionAsync(Model);
        
        // 🔔 Notify My Albums
        WeakReferenceMessenger.Default.Send(new AlbumStartedMessage());
    }
    
    #endregion
}