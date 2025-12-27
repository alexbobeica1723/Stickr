using System.Collections.ObjectModel;
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
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IAlbumsRepository _albumsRepository;

    public CollectionsViewModel(
        IAppInitializationService appInitializationService,
        INavigationService navigationService,
        ICollectionsRepository collectionsRepository,
        IAlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _navigationService = navigationService;
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
                new CollectionItemViewModel(c, _collectionsRepository, _albumsRepository));
        }

        IsBusy = false;
    }
    
    #endregion
}