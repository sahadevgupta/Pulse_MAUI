using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IBlobStorageService
    {
        void ClearLocalStorage();
        Task RetrieveBlobToLocal(Models.Database.Item item);
        Task PushLocalToBlob(Models.Database.Item item);
    }
}