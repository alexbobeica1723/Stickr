using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Stickr.Models;
using Stickr.Services.Repositories;
using Stickr.ViewModels.Base;
using Page =  Stickr.Models.Page;

namespace Stickr.ViewModels.Pages;

public partial class CreateCollectionViewModel : BaseModalPageViewModel
{
    private readonly CollectionsRepository _collectionsRepository;

    public CreateCollectionViewModel(
        CollectionsRepository collectionsRepository)
    {
        _collectionsRepository = collectionsRepository;
    }

    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string description = string.Empty;
    [ObservableProperty] private string? stickerRegex;
    [ObservableProperty] private string imagePath = string.Empty;
    [ObservableProperty] private bool startCollecting;
    
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

        // Re-number pages
        for (int i = 0; i < Pages.Count; i++)
            Pages[i].Number = i + 1;
    }
    
    [RelayCommand]
    private async Task PickImageAsync()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null)
            return;

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
            return;

        var collection = new Collection
        {
            Title = Title,
            Description = Description,
            Image = ImagePath,
            TotalStickers = Pages.Last().LastSticker,
            StickerRegexPattern = string.IsNullOrWhiteSpace(StickerRegex)
                ? @"^\d{1,3}$" // default
                : StickerRegex,
            Pages = Pages.ToList(),
            IsCollecting = false
        };

        await _collectionsRepository.InsertAsync(collection);

        await Shell.Current.GoToAsync("..");
    }

    public ObservableCollection<Page> Pages { get; } = new();

    public override Task InitializeDataAsync()
    {
        // Start with one empty page by default
        Pages.Add(new Page { Number = 1 });
        return Task.CompletedTask;
    }
}