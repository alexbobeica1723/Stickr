using Plugin.Maui.OCR;

namespace Stickr;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        // First push test
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        
        await OcrPlugin.Default.InitAsync();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await CheckCameraPermission();
            if (!hasPermission)
            {
                await DisplayAlert("Permission denied", "Camera permission is required to scan stickers.", "OK");
                return;
            }
            
            var pickResult = await MediaPicker.CapturePhotoAsync();

            if (pickResult != null)
            {
                await using var imageAsStream = await pickResult.OpenReadAsync();
                var imageAsBytes = new byte[imageAsStream.Length];
                await imageAsStream.ReadAsync(imageAsBytes);
                
                var ocrResult = await OcrPlugin.Default.RecognizeTextAsync(imageAsBytes);

                if (!ocrResult.Success)
                {
                    await DisplayAlert("Error", "OCR failed", "OK");
                    return;
                }
                
                await DisplayAlert("Result", ocrResult.Elements[0].Text, "OK");
            }
        }
        catch
        {
            await DisplayAlert("Error", "An error occured", "OK");
        }
    }
    
    private async Task<bool> CheckCameraPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }
        return status == PermissionStatus.Granted;
    }
}