using Stickr.Services.Interfaces;

namespace Stickr.Services.Implementations;

public class PermissionsService : IPermissionsService
{
    public async Task<PermissionStatus> CheckCameraPermissionAsync()
    {
        return await Permissions.CheckStatusAsync<Permissions.Camera>();
    }

    public async Task<PermissionStatus> RequestCameraPermissionAsync()
    {
        return await Permissions.RequestAsync<Permissions.Camera>();
    }
}