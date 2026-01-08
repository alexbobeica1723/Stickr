using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Constants;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Elements;

public partial class CollectionItemViewModel : BaseViewModel
{
    #region Properties
    
    public Collection Model { get; }
    [ObservableProperty]
    private bool _isCollecting;
    [ObservableProperty]
    private bool _isStartCollectionIconVisible;

    public string StatusText =>
        IsCollecting ? "Status: Collecting" : "Status: Available";
    public string TotalStickers => "Stickers to collect: " + Model.TotalStickers;
    
    #endregion

    #region Commands
    
    public IAsyncRelayCommand StartCollectingCommand { get; }
    
    [RelayCommand]
    private async Task OpenCollectionDetailsAsync()
    {
        if (!IsCollecting)
        {
            return;
        }
        
        await _navigationService.NavigateWithOneParameterAsync(NavigationRoutes.CollectionDetailsPage,
            NavigationParameters.CollectionId, Model.Id);
    }
    
    #endregion

    #region Constructor & Dependencies
    
    private readonly ICollectionsRepository _collectionsRepository;
    private readonly IDisplayAlertService _displayAlertService;
    private readonly INavigationService _navigationService;
    
    public CollectionItemViewModel(
        Collection model,
        ICollectionsRepository collectionsRepository,
        IDisplayAlertService displayAlertService,
        INavigationService navigationService)
    {
        Model = model;
        _collectionsRepository =  collectionsRepository;
        _displayAlertService = displayAlertService;
        _navigationService = navigationService;

        IsCollecting = model.IsCollecting;
        IsStartCollectionIconVisible = !IsCollecting;
        StartCollectingCommand = new AsyncRelayCommand(StartCollectingAsync);
    }
    
    #endregion
    
    #region Private Methods

    partial void OnIsCollectingChanged(bool value)
    {
        OnPropertyChanged(nameof(StatusText));
    }

    private async Task StartCollectingAsync()
    {
        var confirm = await _displayAlertService.DisplayAlert(
            "Start collection?",
            $"Do you wish to start collecting " + Model.Title + "?",
            "Yes",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        IsStartCollectionIconVisible = false;

        Model.IsCollecting = true;
        IsCollecting = true;

        await _collectionsRepository.UpdateCollectionAsync(Model);
    }
    
    #endregion
}