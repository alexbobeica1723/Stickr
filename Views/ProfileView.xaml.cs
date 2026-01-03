using Stickr.ViewModels;

namespace Stickr.Views;

public partial class ProfileView : ContentPage
{
    public ProfileView(ProfileViewModel profileViewModel)
    {
        InitializeComponent();
        
        BindingContext = profileViewModel;
    }
}