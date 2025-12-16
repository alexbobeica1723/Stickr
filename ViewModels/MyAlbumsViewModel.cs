using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels;

public partial class MyAlbumsViewModel : BasePageViewModel
{
    private readonly AlbumsRepository _albumsRepository;

    public ObservableCollection<Album> Albums { get; } = new();

    public MyAlbumsViewModel(
        AppInitializationService appInitializationService,
        AlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _albumsRepository = albumsRepository;
        WeakReferenceMessenger.Default.Register<AlbumStartedMessage>(this, Receive);
    }
    
    public async void Receive(object recipient, AlbumStartedMessage message)
    {
        await InitializeDataAsync();
    }

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;

        Albums.Clear();
        var albums = await _albumsRepository.GetAllAsync();

        foreach (var album in albums)
            Albums.Add(album);

        IsBusy = false;
    }
}