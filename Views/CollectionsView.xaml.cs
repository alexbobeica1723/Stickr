using Stickr.ViewModels;

namespace Stickr.Views;

public partial class CollectionsView : ContentPage
{
    public CollectionsView(CollectionsViewModel collectionsViewModel)
    {
        InitializeComponent();
        
        BindingContext = collectionsViewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CollectionsViewModel collectionsViewModel)
        {
            await collectionsViewModel.InitializeDataAsync();
        }
    }
}