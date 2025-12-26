using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class CollectionItemViewModel : BaseViewModel
{
    #region Properties
    
    public Collection Model { get; }
    [ObservableProperty]
    private bool isCollecting;

    public string StatusText =>
        IsCollecting ? "Status: collecting" : "Status: not started";
    
    #endregion

    #region Commands
    
    public IAsyncRelayCommand StartCollectingCommand { get; }
    
    #endregion

    #region Constructor & Dependencies
    
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IAlbumsRepository _albumsRepository;
    
    public CollectionItemViewModel(
        Collection model,
        ICollectionsRepository collectionsRepository,
        IAlbumsRepository albumsRepository)
    {
        Model = model;
        _collectionsRepository =  collectionsRepository;
        _albumsRepository = albumsRepository;

        isCollecting = model.IsCollecting;
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
        if (IsCollecting)
            return;

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