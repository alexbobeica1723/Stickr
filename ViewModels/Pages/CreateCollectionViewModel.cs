using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.Repositories.Interfaces;
using Stickr.Services.Interfaces;
using Stickr.ViewModels.Base;
using Page =  Stickr.Models.Page;

namespace Stickr.ViewModels.Pages;

public partial class CreateCollectionViewModel : BaseModalPageViewModel
{
    #region Properties
    
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string? stickerRegex;
    [ObservableProperty] private string imagePath = string.Empty;
    [ObservableProperty] private bool startCollecting;
    public ObservableCollection<Page> Pages { get; } = new();
    
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private void AddPage()
    {
        Pages.Add(new Page
        {
            Number = Pages.Count + 1
        });
    }
    
    [RelayCommand]
    private void RemovePage(Page page)
    {
        Pages.Remove(page);

        for (var i = 0; i < Pages.Count; i++)
        {
            Pages[i].Number = i + 1;
        }
    }
    
    [RelayCommand]
    private async Task PickImageAsync()
    {
        var result = await MediaPicker.PickPhotoAsync();
        
        if (result is null)
        {
            return;      
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(result.FileName)}";
        var destinationPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        await using var source = await result.OpenReadAsync();
        await using var destination = File.OpenWrite(destinationPath);
        await source.CopyToAsync(destination);

        ImagePath = destinationPath;
    }
    
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            return;
        }

        var collection = new Collection
        {
            Title = Title,
            Description = Description,
            Image = ImagePath,
            TotalStickers = Pages.Last().LastSticker,
            // Default regex is defined for numerical values (values from 1 to 999)
            StickerRegexPattern = @"^\d{1,3}$",
            Pages = Pages.ToList(),
            IsCollecting = false
        };

        await _collectionsRepository.InsertCollectionAsync(collection);
        await _navigationService.GoBackAsync();
    }
    
    #endregion
    
    #region Constructor & Dependencies
    
    private readonly INavigationService _navigationService;
    private readonly ICollectionsRepository _collectionsRepository;

    public CreateCollectionViewModel(
        INavigationService navigationService,
        ICollectionsRepository collectionsRepository)
    {
        _navigationService = navigationService;
        _collectionsRepository = collectionsRepository;
    }
    
    #endregion

    #region Public Methods
    
    public override Task InitializeDataAsync()
    {
        // Start with one empty page by default
        Pages.Add(new Page { Number = 1 });
        return Task.CompletedTask;
    }
    
    #endregion
}