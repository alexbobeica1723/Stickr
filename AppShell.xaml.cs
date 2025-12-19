using Stickr.Views.Pages;

namespace Stickr;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(AlbumDetailsView), typeof(AlbumDetailsView));
        Routing.RegisterRoute(nameof(CreateCollectionView), typeof(CreateCollectionView));
    }
}