using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using Stickr.Constants;
using Stickr.Messages;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;
using Stickr.ViewModels.Elements;

namespace Stickr.ViewModels;

public partial class MyAlbumsViewModel : BaseTabViewModel
{
    private readonly IAlbumsRepository _albumsRepository;
    private readonly INavigationService _navigationService;

    public ObservableCollection<AlbumItemViewModel> Albums { get; } = new();
    
    public ICommand OpenAlbumCommand { get; }

    public MyAlbumsViewModel(
        IAppInitializationService appInitializationService,
        INavigationService navigationService,
        IAlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _albumsRepository = albumsRepository;
        _navigationService = navigationService;
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
        var albums = await _albumsRepository.GetAlbumsAsync();

        foreach (var album in albums)
        {
            Albums.Add(new AlbumItemViewModel(_navigationService, album));
        }

        IsBusy = false;
    }
    
    private async void OnOpenAlbum(Album album)
    {
        if (album is null)
            return;

        await _navigationService.NavigateWithOneParameterAsync(
            NavigationRoutes.AlbumDetailsPage,
            NavigationParameters.AlbumId,
            album.CollectionId);
    }
}