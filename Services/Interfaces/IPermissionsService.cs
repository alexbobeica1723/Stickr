namespace Stickr.Services.Interfaces;

public interface IPermissionsService
{
    Task<PermissionStatus> CheckCameraPermissionAsync();
    Task<PermissionStatus> RequestCameraPermissionAsync();
}