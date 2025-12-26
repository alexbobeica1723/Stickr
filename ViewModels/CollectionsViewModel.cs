using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Repositories.Interfaces;
using Stickr.ViewModels.Base;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Elements;
using Stickr.Views.Pages;

namespace Stickr.ViewModels;

public partial class CollectionsViewModel : BasePageViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IAlbumsRepository _albumsRepository;
    
    [RelayCommand]
    private async Task CreateCollectionAsync()
    {
        await _navigationService.NavigateAsync(NavigationRoutes.CreateCollectionPage);
    }

    public ObservableCollection<CollectionItemViewModel> Collections { get; } = new();

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
}