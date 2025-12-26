namespace Stickr.Services.Interfaces;

public interface IAppInitializationService
{
    Task InitializeAsync();
    Task CompleteInitializationAsync();
}