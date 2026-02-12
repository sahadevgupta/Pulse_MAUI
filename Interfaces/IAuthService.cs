using Pulse_MAUI.Models.Response;

namespace Pulse_MAUI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthInfoDto> Auth(string azureMobileServiceUrl);
        Task<bool> IsTokenAvailable();
        Task Logoff();
        T ProcessException<T>(string message, Exception exception, bool doThrow = true);
        Task<AuthInfoDto> ValidateAuth(string azureMobileServiceUrl);
    }
}