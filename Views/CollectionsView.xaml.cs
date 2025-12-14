using Stickr.ViewModels;

namespace Stickr.Views;

public partial class CollectionsView : ContentPage
{
    public CollectionsView(CollectionsViewModel collectionsViewModel)
    {
        InitializeComponent();
        
        BindingContext = collectionsViewModel;
    }
}