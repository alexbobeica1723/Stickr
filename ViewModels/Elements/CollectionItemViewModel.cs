using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class CollectionItemViewModel : BaseViewModel
{
    private readonly CollectionsRepository _collectionsRepo;
    private readonly AlbumsRepository _albumsRepo;

    public Collection Model { get; }

    [ObservableProperty]
    private bool isCollecting;

    public string StatusText =>
        IsCollecting ? "Status: collecting" : "Status: not started";

    public IAsyncRelayCommand StartCollectingCommand { get; }

    public CollectionItemViewModel(
        Collection model,
        CollectionsRepository collectionsRepo,
        AlbumsRepository albumsRepo)
    {
        Model = model;
        _collectionsRepo = collectionsRepo;
        _albumsRepo = albumsRepo;

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
            TotalStickers = Model.TotalStickers
        };

        await _albumsRepo.InsertAsync(album);

        Model.IsCollecting = true;
        IsCollecting = true;

        await _collectionsRepo.UpdateAsync(Model);
    }
}