using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

[QueryProperty(nameof(AlbumId), "albumId")]
public partial class AlbumDetailsView : ContentPage
{
    private readonly AlbumDetailsViewModel _viewModel;

    public AlbumDetailsView(AlbumDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private string _albumId = string.Empty;

    public string AlbumId
    {
        get => _albumId;
        set
        {
            _albumId = value;
            _viewModel.SetAlbumId(value);
            _ = _viewModel.InitializeAsync();
        }
    }
}