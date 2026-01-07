using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Elements;

namespace Stickr.ViewModels;

public partial class CollectionsViewModel : BaseTabViewModel
{
    #region Properties
    
    public ObservableCollection<CollectionItemViewModel> Collections { get; } = new();
    [ObservableProperty] private bool _isEmpty;
    
    #endregion

    #region Commands
    
    [RelayCommand]
    private async Task CreateCollectionAsync()
    {
        await _navigationService.NavigateAsync(NavigationRoutes.CreateCollectionPage);
    }
    
    #endregion
    
    #region Constructor & Dependencies

    private readonly INavigationService _navigationService;
    private readonly IDisplayAlertService _displayAlertService;
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IAlbumsRepository _albumsRepository;

    public CollectionsViewModel(
        IAppInitializationService appInitializationService,
        INavigationService navigationService,
        IDisplayAlertService displayAlertService,
        ICollectionsRepository collectionsRepository,
        IAlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _navigationService = navigationService;
        _displayAlertService = displayAlertService;
        _collectionsRepository = collectionsRepository;
        _albumsRepository = albumsRepository;
    }
    
    #endregion
    
    #region Public Methods

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;

        Collections.Clear();

        var data = await _collectionsRepository.GetCollectionsAsync();
        foreach (var c in data)
        {
            Collections.Add(
                new CollectionItemViewModel(c, _collectionsRepository, _albumsRepository, _displayAlertService));
        }
        
        IsEmpty = data.Count == 0;

        IsBusy = false;
    }
    
    #endregion
}