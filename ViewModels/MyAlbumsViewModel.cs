using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
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
    #region Properties
    
    public ObservableCollection<AlbumItemViewModel> Albums { get; } = new();
    [ObservableProperty]
    private bool _isEmpty;
    
    #endregion

    #region Commands
    
    public ICommand OpenAlbumCommand { get; }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly IAlbumsRepository _albumsRepository;
    private readonly IStickersRepository _stickersRepository;
    private readonly INavigationService _navigationService;

    public MyAlbumsViewModel(
        IAppInitializationService appInitializationService,
        INavigationService navigationService,
        IAlbumsRepository albumsRepository,
        IStickersRepository stickersRepository)
        : base(appInitializationService)
    {
        _albumsRepository = albumsRepository;
        _stickersRepository = stickersRepository;
        _navigationService = navigationService;
        WeakReferenceMessenger.Default.Register<AlbumStartedMessage>(this, Receive);
        OpenAlbumCommand = new Command<Album>(OnOpenAlbum);
    }
    
    #endregion
    
    #region Public Methods
    
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
            Albums.Add(new AlbumItemViewModel(_navigationService, album, _stickersRepository));
        }
        
        IsEmpty = albums.Count == 0;

        IsBusy = false;
    }
    
    #endregion

    #region Private Methods

    private async void OnOpenAlbum(Album? album)
    {
        if (album is null)
        {
            return;
        }

        await _navigationService.NavigateWithOneParameterAsync(
            NavigationRoutes.AlbumDetailsPage,
            NavigationParameters.AlbumId,
            album.CollectionId);
    }

    #endregion
}