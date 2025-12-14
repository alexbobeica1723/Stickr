using Plugin.Maui.OCR;
using IOcrService = Stickr.Services.Interfaces.IOcrService;

namespace Stickr.Services.Implementations;

public class OcrService : IOcrService
{
    public async Task<string> RecognizeTextAsync(byte[] imageBytes)
    {
        var result = await OcrPlugin.Default.RecognizeTextAsync(imageBytes);

        if (!result.Success || result.Elements.Count == 0)
            return string.Empty;

        return string.Join(" ", result.Elements.Select(e => e.Text));
    }
}