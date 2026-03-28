using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;

using PCATablet.Core.Helpers;
using PCATablet.Core.Models;
using PCATablet.Core.Interfaces;


namespace PCATablet.Core.Data
{
	/// <summary>
	/// Data Manager class, handles all azure app service related functionality.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public partial class DataManagerItems
	{
		static DataManagerItems defaultInstance = new DataManagerItems();

		MobileServiceClient client;
		MobileServiceSQLiteStore localStore;
		MySyncHandler mySyncHandler;
	
        IMobileServiceSyncTable<Item> itemTable;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:PCATablet.Core.Data.DataManager"/> class.
        /// </summary>
        public DataManagerItems()
		{
			this.client = new MobileServiceClient(Keys.AzureService, new HttpMessageHandler[]{new AuthHeaderHandler()});
            this.client = DataManager.DefaultManager.CurrentClient;

			string path = "PCAOfflineCache.db";

			localStore = new MobileServiceSQLiteStore(path);

            localStore.DefineTable<Item>();
            this.itemTable = client.GetSyncTable<Item>();
 

            mySyncHandler = new MySyncHandler(client);
			this.client.SyncContext.InitializeAsync(localStore, mySyncHandler);
		}

		/// <summary>
		/// Gets or sets the default manager.
		/// </summary>
		/// <value>The default manager.</value>
		public static DataManagerItems DefaultManager
		{
			get
			{
				return defaultInstance;
			}
			set
			{
				defaultInstance = value;
			}
		}

		/// <summary>
		/// Gets the current client.
		/// </summary>
		/// <value>The current client.</value>
		public MobileServiceClient CurrentClient
		{
			get { return client; }
            set { client = value; }
		}

		#region Authentication
		/// <summary>
		/// Logs the user into the mobile client and server.
		/// </summary>
		/// <returns>async task.</returns>
		public Task LoginAsync()
		{
			var loginProvider = DependencyService.Get<ILoginProvider>();
			return loginProvider.LoginAsync(client, defaultInstance);
		}

        public Task LogoutAsync()
        {


            var loginProvider = DependencyService.Get<ILoginProvider>();
            return loginProvider.LogoutAsync(client);
        }

		#endregion

		#region Get

        /// <summary>
        /// Gets all items async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await itemTable
                .ToEnumerableAsync();
        }

       

        #endregion

        #region Update
  
        /// <summary>
		/// Saves the Item async.
		/// </summary>
		/// <returns>async task.</returns>
		/// <param name="Item">Item to save.</param>
        public async Task SaveItemAsync(Item item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                await itemTable.InsertAsync(item);
            }
            else
            {
                if (item.IsDirty)
                {
                    await itemTable.UpdateAsync(item);
                }
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete the Item async.
        /// </summary>
        /// <returns>async task.</returns>
        /// <param name="Item">Item to delete.</param>
        public async Task DeleteItemAsync(Item item)
        {
           await itemTable.DeleteAsync(item);
        }

        #endregion

        #region Synchronisation


        /// <summary>
        /// Synchronizes the push and pull items asynchronous.
        /// </summary>
        /// <param name="purge">if set to <c>true</c> [purge].</param>
        /// <returns></returns>
        public async Task SyncPushAndPullAsync()
        {

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try { 
       
                await this.itemTable.PurgeAsync();
                await this.itemTable.PullAsync(null,this.itemTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
        }



		#endregion

	}
}
