using Stickr.ViewModels;

namespace Stickr.Views;

public partial class CollectionsView : ContentPage
{
    private readonly CollectionsViewModel _collectionsViewModel;
    
    public CollectionsView(CollectionsViewModel collectionsViewModel)
    {
        InitializeComponent();
        
        _collectionsViewModel = collectionsViewModel;
        BindingContext = _collectionsViewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_collectionsViewModel.Collections.Any())
            _collectionsViewModel.LoadCollectionsCommand.Execute(null);
    }
}