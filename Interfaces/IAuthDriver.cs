using Pulse_MAUI.Models.Response;

namespace Pulse_MAUI.Interfaces
{
    public interface IAuthDriver
    {
        Task<AuthResultDto> AuthenticateUser();
        Task Clear();
        Task SignOutAsync();
    }
}