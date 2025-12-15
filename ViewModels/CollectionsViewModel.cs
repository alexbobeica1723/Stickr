using System.Collections.ObjectModel;
using System.Windows.Input;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels;

public class CollectionsViewModel : BasePageViewModel
{
    private readonly DatabaseService _databaseService;
    
    public string Title => "Collections";
    
    public ObservableCollection<Collection> Collections { get; } = new();

    public CollectionsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
    
    private async Task LoadCollectionsAsync()
    {
        IsBusy = true;
        
        try
        {
            Collections.Clear();

            var collections = await _databaseService.GetCollectionsAsync();

            foreach (var collection in collections)
                Collections.Add(collection);
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected override async Task InitializeDataAsync()
    {
        await LoadCollectionsAsync();
    }
}