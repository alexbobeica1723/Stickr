using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class AlbumDetailsView : ContentPage
{
    public AlbumDetailsView(AlbumDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumDetailsViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}