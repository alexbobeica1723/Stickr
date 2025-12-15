using System.Collections.ObjectModel;
using System.Windows.Input;
using Stickr.Models;
using Stickr.Services.Implementations;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels;

public class CollectionsViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    
    public string Title => "Collections";
    
    public ObservableCollection<Collection> Collections { get; } = new();
    
    public ICommand LoadCollectionsCommand { get; }

    public CollectionsViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        
        LoadCollectionsCommand = new Command(async () => await LoadCollectionsAsync());
    }
    
    private async Task LoadCollectionsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
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
}