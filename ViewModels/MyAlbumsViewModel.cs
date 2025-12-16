using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using Stickr.ViewModels.Elements;
using Stickr.Views.Pages;

namespace Stickr.ViewModels;

public partial class MyAlbumsViewModel : BasePageViewModel
{
    private readonly AlbumsRepository _albumsRepository;

    public ObservableCollection<AlbumItemViewModel> Albums { get; } = new();
    
    public ICommand OpenAlbumCommand { get; }

    public MyAlbumsViewModel(
        AppInitializationService appInitializationService,
        AlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _albumsRepository = albumsRepository;
        WeakReferenceMessenger.Default.Register<AlbumStartedMessage>(this, Receive);
        OpenAlbumCommand = new Command<Album>(OnOpenAlbum);
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
        {
            Albums.Add(new AlbumItemViewModel(album));
        }

        IsBusy = false;
    }
    
    private async void OnOpenAlbum(Album album)
    {
        if (album == null)
            return;

        await Shell.Current.GoToAsync(
            $"{nameof(AlbumDetailsView)}?albumId={album.CollectionId}");
    }
}