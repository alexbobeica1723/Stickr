using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class NavigationService : INavigationService
{
    public async Task NavigateAsync(string route)
    {
        await Shell.Current.GoToAsync(route);
    }

    public async Task NavigateWithOneParameterAsync(string route, string parameterName, object parameter)
    {
        await Shell.Current.GoToAsync($"{route}?{parameterName}={parameter}");
    }

    public async Task NavigateWithMultipleParametersAsync(string route, Dictionary<string, object> parametersDictionary)
    {
        await Shell.Current.GoToAsync(route, parametersDictionary);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}