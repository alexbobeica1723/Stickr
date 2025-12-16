using CommunityToolkit.Mvvm.ComponentModel;

namespace Stickr.ViewModels.Base;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;
}