using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IBlobStorageService
    {
        void ClearLocalStorage();
        Task RetrieveBlobToLocal(Item item);
        Task PushLocalToBlob(Item item);
    }
}