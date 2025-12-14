namespace Stickr.Services.Interfaces;

public interface IOcrService
{
    Task<string> RecognizeTextAsync(byte[] imageBytes);
}