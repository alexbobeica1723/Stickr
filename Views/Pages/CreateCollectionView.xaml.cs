using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class CreateCollectionView : ContentPage
{
    public CreateCollectionView(CreateCollectionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CreateCollectionViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}