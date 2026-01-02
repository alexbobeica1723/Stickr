using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.ViewModels.Base;

namespace Stickr.ViewModels.Pages;

public partial class OnboardingViewModel : BaseModalPageViewModel
{
    #region Properties
    
    public ObservableCollection<OnboardingSlide> Slides { get; set; }
    [ObservableProperty]
    private int _position;
    [ObservableProperty]
    private bool _isBackButtonVisible;
    [ObservableProperty]
    private bool _isFinishButtonVisible;
    
    #endregion
    
    #region Commands

    [RelayCommand]
    private void Next()
    {
        Position++;
        UpdateButtonsVisibility();
    }

    [RelayCommand]
    private void Previous()
    {
        Position--;
        UpdateButtonsVisibility();
    }

    [RelayCommand]
    private async Task FinishAsync()
    {
        Preferences.Set("OnboardingCompleted", true);
        await Shell.Current.Navigation.PopModalAsync();
    }
    
    #endregion
    
    #region Constructor & Dependencies

    public OnboardingViewModel()
    {
        Slides = new ObservableCollection<OnboardingSlide>
        {
            new()
            {
                Title = "Welcome to Stickr",
                Description = "Track your sticker collections easily and visually.",
                Image = "onboarding1.png"
            },
            new()
            {
                Title = "Scan Stickers",
                Description = "Use your camera to scan and add stickers instantly.",
                Image = "onboarding2.png"
            },
            new()
            {
                Title = "Track Progress",
                Description = "See statistics and completion progress per album.",
                Image = "onboarding3.png"
            }
        };
    }
    
    #endregion
    
    #region Public Methods
    
    public override Task InitializeDataAsync()
    {
        return Task.CompletedTask;
    }
    
    #endregion
    
    #region Private Methods
    
    private void UpdateButtonsVisibility()
    {
        IsBackButtonVisible = Position > 0;
        IsFinishButtonVisible = Position == Slides.Count - 1;
    }
    
    #endregion
}