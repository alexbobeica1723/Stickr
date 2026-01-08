using Stickr.Views.Pages;

namespace Stickr;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(CollectionDetailsView), typeof(CollectionDetailsView));
        Routing.RegisterRoute(nameof(CreateCollectionView), typeof(CreateCollectionView));
        Routing.RegisterRoute(nameof(StickerDetailsView), typeof(StickerDetailsView));
        Routing.RegisterRoute(nameof(CollectionStatsView), typeof(CollectionStatsView));
    }
}