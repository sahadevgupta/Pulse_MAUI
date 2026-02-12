using Microsoft.Datasync.Client;
using PCATablet.Core.Data;
using Pulse_MAUI.Data;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
	/// <summary>
	/// Interface with the contract for a Login provider.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public interface ILoginProvider
	{
        /// <summary>
        /// Login async method definition.
        /// </summary>
        /// <returns>Task.</returns>
        /// <param name="client">Azure Mobile Service client.</param>
        //Task<DataSyncUser> LoginAsync(DatasyncClient client, DataManager dataManager);
        Task<MobileServiceUser> LoginAsync(DatasyncClient client, IDataManager dataManager, string azureMobileServiceUrl);
        //Task LoginAsync(MobileServiceClient client, DataManagerItems defaultInstance);

        /// <summary>
        /// Logout async method definition.
        /// </summary>
        /// <returns>Task.</returns>
        /// <param name="client">Azure Mobile Service client.</param>
        Task LogoutAsync(DatasyncClient client);
    

    }
}
