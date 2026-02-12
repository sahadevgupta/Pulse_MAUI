using Pulse_MAUI.Models.Request;

namespace Pulse_MAUI.Interfaces
{
    public interface IProjectServices
    {
        Task<string> GetAppConfigAsync();
        Task<string> GetAzureConnectionAsync();
        Task PostSyncLogAsync(SyncLogRequest syncLogRequest);
    }
}