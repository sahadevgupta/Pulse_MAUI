using Microsoft.Datasync.Client;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
	public interface ILoginProvider
	{
        Task<MobileServiceUser> LoginAsync(string azureMobileServiceUrl);
        Task LogoutAsync(DatasyncClient client);
    }
}
