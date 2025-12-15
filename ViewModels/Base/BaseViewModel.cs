using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Stickr.ViewModels.Base;

public class BaseViewModel : INotifyPropertyChanged
{
    private bool _isBusy;

    protected bool IsBusy
    {
        get => _isBusy;
        set => SetField(ref _isBusy, value);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}