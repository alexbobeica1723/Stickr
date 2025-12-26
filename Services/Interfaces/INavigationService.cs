namespace Stickr.Services.Interfaces;

public interface INavigationService
{
    Task NavigateAsync(string route);
    Task NavigateWithOneParameterAsync(string route, string parameterName, object parameter);
    Task NavigateWithMultipleParametersAsync(string route, Dictionary<string, object> parametersDictionary);
    Task GoBackAsync();
}