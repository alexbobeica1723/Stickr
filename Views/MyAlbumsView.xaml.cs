using Stickr.ViewModels;

namespace Stickr.Views;

public partial class MyAlbumsView : ContentPage
{
    public MyAlbumsView(MyAlbumsViewModel myAlbumsViewModel)
    {
        InitializeComponent();
        
        BindingContext = myAlbumsViewModel;
    }
}