namespace Stickr.Services.Interfaces;

public interface IDisplayAlertService
{
    Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
    Task DisplayCancelOnlyAlert(string title, string message, string cancel);
}