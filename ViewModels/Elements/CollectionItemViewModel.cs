using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class CollectionItemViewModel : BaseViewModel
{
    private readonly CollectionsRepository _collectionsRepository;
    private readonly AlbumsRepository _albumsRepository;

    public Collection Model { get; }

    [ObservableProperty]
    private bool isCollecting;

    public string StatusText =>
        IsCollecting ? "Status: collecting" : "Status: not started";

    public IAsyncRelayCommand StartCollectingCommand { get; }

    public CollectionItemViewModel(
        Collection model,
        CollectionsRepository collectionsRepository,
        AlbumsRepository albumsRepository)
    {
        Model = model;
        _collectionsRepository =  collectionsRepository;
        _albumsRepository = albumsRepository;

        isCollecting = model.IsCollecting;

        StartCollectingCommand = new AsyncRelayCommand(StartCollectingAsync);
    }

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

        await _albumsRepository.InsertAsync(album);

        Model.IsCollecting = true;
        IsCollecting = true;

        await _collectionsRepository.UpdateAsync(Model);
        
        // 🔔 Notify My Albums
        WeakReferenceMessenger.Default.Send(new AlbumStartedMessage());
    }
}