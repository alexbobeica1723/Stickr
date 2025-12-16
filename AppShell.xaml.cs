using Stickr.Views.Pages;

namespace Stickr;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(AlbumDetailsView), typeof(AlbumDetailsView));
    }
}