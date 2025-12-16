using System.Collections.ObjectModel;
using System.Windows.Input;
using Stickr.Models;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Services.Implementations;
using Stickr.ViewModels.Elements;

namespace Stickr.ViewModels;

public class CollectionsViewModel : BasePageViewModel
{
    private readonly CollectionsRepository _collectionsRepo;
    private readonly AlbumsRepository _albumsRepo;

    public ObservableCollection<CollectionItemViewModel> Collections { get; } = new();

    public CollectionsViewModel(
        AppInitializationService appInit,
        CollectionsRepository repo,
        AlbumsRepository albumsRepo)
        : base(appInit)
    {
        _collectionsRepo = repo;
        _albumsRepo = albumsRepo;
    }

    public override async Task InitializeDataAsync()
    {
        IsBusy = true;

        Collections.Clear();

        var data = await _collectionsRepo.GetAllAsync();
        foreach (var c in data)
        {
            Collections.Add(
                new CollectionItemViewModel(c, _collectionsRepo, _albumsRepo));
        }

        IsBusy = false;
    }
}