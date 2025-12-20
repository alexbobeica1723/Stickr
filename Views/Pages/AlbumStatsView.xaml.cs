using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stickr.ViewModels.Pages;

namespace Stickr.Views.Pages;

public partial class AlbumStatsView : ContentPage
{
    public AlbumStatsView(AlbumStatsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AlbumStatsViewModel viewModel)
        {
            await viewModel.InitializeDataAsync();
        }
    }
}