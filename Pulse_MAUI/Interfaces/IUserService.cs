using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IUserService
    {
        User? CurrentUser { get; }
        Task FetchCurrentUser();
        Task<string> GetAzureBlobStorageString();
        Task LoginAsync(string mobileAzureServiceUrl);
        Task LogoutAsync();
    }
}