using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class OnboardingView : ContentPage
{
    public OnboardingView()
    {
        InitializeComponent();
        
        BindingContext = new OnboardingViewModel();
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is OnboardingViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}