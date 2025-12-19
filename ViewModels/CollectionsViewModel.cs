using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using Stickr.Services.Implementations;
using Stickr.ViewModels.Elements;
using Stickr.Views.Pages;

namespace Stickr.ViewModels;

public partial class CollectionsViewModel : BasePageViewModel
{
    private readonly CollectionsRepository _collectionsRepository;
    private readonly AlbumsRepository _albumsRepository;
    
    [RelayCommand]
    private async Task CreateCollectionAsync()
    {
        await Shell.Current.GoToAsync(nameof(CreateCollectionView));
    }

    public ObservableCollection<CollectionItemViewModel> Collections { get; } = new();

    public CollectionsViewModel(
        AppInitializationService appInitializationService,
        CollectionsRepository collectionsRepository,
        AlbumsRepository albumsRepository)
        : base(appInitializationService)
    {
        _collectionsRepository = collectionsRepository;
        _albumsRepository = albumsRepository;
    }

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;

        Collections.Clear();

        var data = await _collectionsRepository.GetAllAsync();
        foreach (var c in data)
        {
            Collections.Add(
                new CollectionItemViewModel(c, _collectionsRepository, _albumsRepository));
        }

        IsBusy = false;
    }
}