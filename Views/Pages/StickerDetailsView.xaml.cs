using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class StickerDetailsView : ContentPage
{
    public StickerDetailsView(StickerDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StickerDetailsViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}