using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class CollectionDetailsView : ContentPage
{
    public CollectionDetailsView(CollectionDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CollectionDetailsViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}