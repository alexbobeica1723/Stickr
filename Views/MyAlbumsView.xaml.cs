using Stickr.Models;
using Stickr.ViewModels;
using Stickr.Views.Pages;

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
    
    private async void OnAlbumSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Album album)
        {
            await Shell.Current.GoToAsync(
                $"{nameof(AlbumDetailsView)}?albumId={album.CollectionId}");

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}