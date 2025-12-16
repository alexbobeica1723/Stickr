using Stickr.ViewModels;

namespace Stickr.Views;

public partial class MyAlbumsView : ContentPage
{
    public MyAlbumsView(MyAlbumsViewModel myAlbumsViewModel)
    {
        InitializeComponent();
        
        BindingContext = myAlbumsViewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is MyAlbumsViewModel myAlbumsViewModel)
        {
            await myAlbumsViewModel.InitializeDataAsync();
        }
    }
}