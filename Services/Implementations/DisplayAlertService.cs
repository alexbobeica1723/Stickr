using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class DisplayAlertService : IDisplayAlertService
{
    public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }

    public async Task DisplayCancelOnlyAlert(string title, string message, string cancel)
    {
        await Shell.Current.DisplayAlert(title, message, cancel);
    }
}