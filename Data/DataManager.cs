using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Data;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Models.Request;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Activity = Pulse_MAUI.Models.Activity;

namespace Pulse_MAUI.Data
{
    public partial class DataManager : IDataManager
    {
        readonly IProjectServices projectServices;

        private DatasyncClient client;
        private OfflineSQLiteStore localStore;
        private readonly ILoginProvider? loginProvider = IPlatformApplication.Current?.Services.GetRequiredService<ILoginProvider>();

        private IOfflineTable<ActivityTask> activityTaskTable;
        private IOfflineTable<PunchItem> punchItemTable;
        private IOfflineTable<Component> componentTable;
        private IOfflineTable<CommissioningSystem> commissioningSystemTable;
        private IOfflineTable<Project> projectTable;
        private IOfflineTable<Unit> unitTable;
        private IOfflineTable<Activity> activityTable;
        private IOfflineTable<Engineer> engineerTable;
        private IOfflineTable<User> userTable;
        private IOfflineTable<Lookup> lookupTable;
        private IOfflineTable<Item> itemTable;
        private IOfflineTable<Equipment> equipmentTable;
        private IOfflineTable<Priority> priorityTable;
        private IOfflineTable<Discipline> disciplineTable;

        public DataManager(IProjectServices projectServices)
        {
            this.projectServices = projectServices;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await InitDataManager();

            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void InitDataManager(string url)
        {
            var uri = new Uri(url);
            var host = "https://" + uri.Host;

            // implement the mobile service client
            this.client = new DatasyncClient(host, new DatasyncClientOptions
            {
                HttpPipeline = new HttpMessageHandler[] { new AuthHeaderHandler() }

            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PCATablet.Core.Data.DataManager"/> class.
        /// </summary>
        public async Task InitDataManager()
        {
            var folderPath = Path.Combine(AppConstants.AppRootFolder, AppHelpers.BlobStorageName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var dbPath = Path.Combine(folderPath, "PCAOfflineCache.db");
            var sqliteUri = $"file:{dbPath}";
            localStore = new OfflineSQLiteStore(sqliteUri);



            // Configure options
            var options = new DatasyncClientOptions
            {
                HttpPipeline = new HttpMessageHandler[]
                {
                    new AuthHeaderHandler()
                },
                OfflineStore = localStore
            };

            


            //Create client with options
            client = new DatasyncClient(AppHelpers.AzureServiceUrl, options);

            // setup the local store for each of the DataTables
            localStore.DefineTable<Activity>();
            localStore.DefineTable<PunchItem>();
            localStore.DefineTable<ActivityTask>();
            localStore.DefineTable<Component>();
            localStore.DefineTable<Lookup>();
            localStore.DefineTable<CommissioningSystem>();
            localStore.DefineTable<Project>();
            localStore.DefineTable<Unit>();
            localStore.DefineTable<Engineer>();
            localStore.DefineTable<User>();
            localStore.DefineTable<Item>();
            localStore.DefineTable<Equipment>();
            localStore.DefineTable<Priority>();
            localStore.DefineTable<Discipline>();

            // *** FIX: MUST AWAIT THIS ***
            await client.InitializeOfflineStoreAsync();

            // Return each Table type
            this.activityTable = client.GetOfflineTable<Activity>();
            this.activityTaskTable = client.GetOfflineTable<ActivityTask>();
            this.punchItemTable = client.GetOfflineTable<PunchItem>();
            this.componentTable = client.GetOfflineTable<Component>();
            this.commissioningSystemTable = client.GetOfflineTable<CommissioningSystem>();
            this.projectTable = client.GetOfflineTable<Project>();
            this.unitTable = client.GetOfflineTable<Unit>();
            this.engineerTable = client.GetOfflineTable<Engineer>();
            this.userTable = client.GetOfflineTable<User>();
            this.lookupTable = client.GetOfflineTable<Lookup>();
            this.itemTable = client.GetOfflineTable<Item>();
            this.equipmentTable = client.GetOfflineTable<Equipment>();
            this.priorityTable = client.GetOfflineTable<Priority>();
            this.disciplineTable = client.GetOfflineTable<Discipline>();



        }

        /// <summary>
		/// Gets the current client.
		/// </summary>
		/// <value>The current client.</value>
		public DatasyncClient CurrentClient
        {
            get { return client; }
            set { client = value; }
        }

        #region Authentication
        /// <summary>
        /// Logs the user into the mobile client and server.
        /// </summary>
        /// <returns>async task.</returns>
        public async Task<MobileServiceUser> LoginAsync(string azureMobileServiceUrl)
        {
            return await loginProvider?.LoginAsync(client, this, azureMobileServiceUrl)!;
        }

        /// <summary>
        /// Logout from AD asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task? LogoutAsync()
        {
            return loginProvider?.LogoutAsync(client);


        }

        #endregion

        #region Get

        /// <summary>
        /// Gets all disciplines.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Discipline>> GetAllDisciplines()
        {
            
            return await disciplineTable.ToListAsync();
        }

        /// <summary>
        /// Gets all activities async.
        /// </summary>
        /// <returns>All activities async.</returns>
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            //InitDataManager();
            return await activityTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all activity tasks for activity async.
        /// </summary>
        /// <returns>All activity tasks.</returns>
        /// <param name="activity">Activity to get activity tasks for.</param>
        public async Task<IEnumerable<ActivityTask>> GetAllActivityTasksForActivityAsync(Activity activity)
        {
            
            return await activityTaskTable
                .Where(p => p.ActivityId == activity.pcaId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the activity task by identifier.
        /// </summary>
        /// <returns>The activity task by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<ActivityTask> GetActivityTaskById(string id)
        {
            
            var activityTask = await activityTaskTable
                .Where(p => p.Id == id)
                .ToListAsync();

            return activityTask.FirstOrDefault();
        }


        /// <summary>
        /// Gets the activity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Activity> GetActivityById(string id)
        {
            
            var activity = await activityTable
                .Where(p => p.id == id)
                .ToListAsync();

            return activity.FirstOrDefault();
        }

        /// <summary>
        /// Gets all punch items async.
        /// </summary>
        /// <returns>All punch items async.</returns>
        public async Task<IEnumerable<PunchItem>> GetAllPunchItemsAsync()
        {
            
            return await punchItemTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets the punch item by identifier.
        /// </summary>
        /// <returns>The punch item by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<PunchItem> GetPunchItemById(string id)
        {
            
            var punchItem = await punchItemTable
                .Where(p => p.Id == id)
                .ToListAsync();

            return punchItem.FirstOrDefault();
        }

        /// <summary>
        /// Gets all engineers async.
        /// </summary>
        /// <returns>All Engineers async.</returns>
        public async Task<IEnumerable<Engineer>> GetAllEngineersAsync()
        {
            
            return await engineerTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all users async.
        /// </summary>
        /// <returns>All Users async.</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            
            return await userTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all projects async.
        /// </summary>
        /// <returns>The all projects async.</returns>
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            
            var a = await projectTable
                .ToListAsync();

            return a;
        }

        /// <summary>
        /// Gets all units async.
        /// </summary>
        /// <returns>All units async.</returns>
        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            
            return await unitTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all commissioning systems async.
        /// </summary>
        /// <returns>All commissioning systems async.</returns>
        public async Task<IEnumerable<CommissioningSystem>> GetAllCommissioningSystemsAsync()
        {
            
            return await commissioningSystemTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all components async.
        /// </summary>
        /// <returns>All components async.</returns>
        public async Task<IEnumerable<Component>> GetAllComponentsAsync()
        {
            
            return await componentTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all lookups async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync()
        {
            
            return await lookupTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all items async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            
            return await itemTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all equipment asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            
            return await equipmentTable
                 .ToListAsync();

        }

        /// <summary>
        /// Gets all priority.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Priority>> GetAllPriority()
        {
            
            return await priorityTable
                 .ToListAsync();
        }

        #endregion

        #region Update
        /// <summary>
        /// Saves an activity task async.
        /// </summary>
        /// <returns>async Task.</returns>
        /// <param name="activityTask">Activity task to save.</param>
        public async Task SaveActivityTaskAsync(ActivityTask activityTask)
        {
            
            await activityTaskTable.ReplaceItemAsync(activityTask);
        }

        /// <summary>
        /// Saves the activity asynchronous.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        public async Task SaveActivityAsync(Activity activity)
        {
            
            await activityTable.ReplaceItemAsync(activity);
        }

        /// <summary>
        /// Saves the punch item async.
        /// </summary>
        /// <returns>async task.</returns>
        /// <param name="punchItem">Punch item to save.</param>
        public async Task SavePunchItemAsync(PunchItem punchItem)
        {
            
            if (string.IsNullOrEmpty(punchItem.Id))
            {
                await punchItemTable.InsertItemAsync(punchItem);
            }
            else
            {
                await punchItemTable.ReplaceItemAsync(punchItem);
            }
        }

        /// <summary>
		/// Saves the Item async.
		/// </summary>
		/// <returns>async task.</returns>
		/// <param name="Item">Item to save.</param>
        public async Task SaveItemAsync(Item item)
        {
            
            if (string.IsNullOrEmpty(item.Id))
            {
                try
                {
                    await itemTable.InsertItemAsync(item);
                }
                catch (Exception ex)
                {
                    var error = ex.ToString();
                }
            }
            else
            {
                if (item.IsDirty)
                {
                    await itemTable.ReplaceItemAsync(item);
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
            
            if (item != null)
            {
                await itemTable.DeleteItemAsync(item);
            }
        }

        #endregion

        #region Synchronisation
        /// <summary>
        /// Dooes a push and pull to the azure backend.
        /// </summary>
        /// <returns>async task.</returns>
        /// <param name="incremental">Do an incremental or full pull of the data</param>
        public async Task<List<string>> SyncPushAndPullItemsAsync(bool incremental, bool secondPass)
        {
             //InitDataManager();
            //ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            List<string> Errors = new List<string>();

            try
            {

                //First do a push
                long? _pendingOperations = this.client.PendingOperations;

                await this.client.PushTablesAsync();


                //await this.itemTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await  itemTable.PullItemsAsync(
    query: itemTable.CreateQuery(),
    cancellationToken: CancellationToken.None
);

                if (!secondPass)
                {


                    await this.activityTable.PurgeItemsAsync(activityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.activityTable.PullItemsAsync(
                             this.activityTable.CreateQuery());

                    await Task.Delay(500);

                    var a =  await this.activityTable.ToListAsync();

                    await this.punchItemTable.PurgeItemsAsync(punchItemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.punchItemTable.PullItemsAsync(
                        this.punchItemTable.CreateQuery());

                    await this.engineerTable.PurgeItemsAsync(engineerTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.engineerTable.PullItemsAsync(
                        this.engineerTable.CreateQuery());

                    await this.userTable.PurgeItemsAsync(userTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.userTable.PullItemsAsync(
                        this.userTable.CreateQuery());

                    await this.projectTable.PurgeItemsAsync(projectTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.projectTable.PullItemsAsync(
                        this.projectTable.CreateQuery());

                    await this.commissioningSystemTable.PurgeItemsAsync(commissioningSystemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.commissioningSystemTable.PullItemsAsync(
                        this.commissioningSystemTable.CreateQuery());

                    await this.unitTable.PurgeItemsAsync(unitTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.unitTable.PullItemsAsync(
                       this.unitTable.CreateQuery());

                    await this.componentTable.PurgeItemsAsync(componentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.componentTable.PullItemsAsync(
                       this.componentTable.CreateQuery());

                    await this.lookupTable.PullItemsAsync(this.lookupTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PullOptions
                    {
                        QueryId = incremental ? "LookupDataIncremental" : null
                    });

                    await this.priorityTable.PurgeItemsAsync(priorityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.priorityTable.PullItemsAsync(
                        this.priorityTable.CreateQuery());

                    await this.activityTaskTable.PurgeItemsAsync(activityTaskTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.activityTaskTable.PullItemsAsync(
                        this.activityTaskTable.CreateQuery());

                    await this.equipmentTable.PurgeItemsAsync(equipmentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.equipmentTable.PullItemsAsync(
                        this.equipmentTable.CreateQuery());


                    await this.disciplineTable.PurgeItemsAsync(disciplineTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    await this.disciplineTable.PullItemsAsync(
                        this.disciplineTable.CreateQuery());

                }

            }
            catch (DatasyncConflictException exc)
            {
                //var table = ex;        // "Tasks"
                //var id = ex.ItemId;              // "123"

                //var client = ex.ClientItem;      // JObject: your local pending version
                //var server = ex.ServerItem;      // JObject: latest server version
                //var original = ex.OriginalItem;  // JObject: previous version

                //if (exc.IsConflictStatusCode != null)
                //{
                //    syncErrors = exc.PushResult.Errors;

                //    foreach (var err in exc.Data.Keys)
                //    {
                //        Errors.Add("(PushFailedException) + Error on Sync Table:  " + err.TableName.ToString());
                //    }

                //}


            }
            catch (DatasyncInvalidOperationException ex)
            {
                var route = ex.Request.RequestUri.AbsolutePath;
                // var x = ex.Response.ToString();
                System.Diagnostics.Debug.WriteLine("Error on: {0}", route);
                Errors.Add("Error on Sync (Invalid Operation) " + ex.ToString());
                var error = ex.Message;

            }
            catch (Exception ex)
            {
                Errors.Add("Error on Sync (Exception) " + ex.ToString());
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            //if (syncErrors != null)
            {
                //foreach (MobileServiceTableOperationError error in syncErrors)
                //{

                //    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                //    {
                //        //Update failed, reverting to server's copy.
                //        await error.CancelAndUpdateItemAsync(error.Result);
                //        Errors.Add("Error" + " updating record " + error.TableName.ToString() + " record");
                //    }
                //    else if (error.OperationKind == MobileServiceTableOperationKind.Insert && error.Result != null)
                //    {
                //        // dont do anything, i.e leave the current record on the device!
                //        Errors.Add("Error" + " inserting new " + error.TableName.ToString() + " record");
                //    }
                //    else
                //    {
                //        Errors.Add("Unknown error of " + error.OperationKind.ToString() + " on table " + error.TableName.ToString());

                //        // Discard local change.
                //        // await error.CancelAndDiscardItemAsync();
                //    }
                //}

            }

            return Errors;
        }

        /// <summary>
        /// Pushes the data to azure and purges the data locally
        /// </summary>
        /// <returns>async task.</returns>
        public async Task SyncPushAndPurgeAsync()
        {
            
            //ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                //First do a push
                await this.client.PushTablesAsync();

                await activityTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await punchItemTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await activityTaskTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await projectTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await commissioningSystemTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await unitTable.PurgeItemsAsync(null, null, CancellationToken.None);
                await componentTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await disciplineTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await itemTable.PurgeItemsAsync(null, null, CancellationToken.None);
            }
            catch (DatasyncConflictException exc)
            {
                //if (exc.PushResult != null)
                //{
                //    syncErrors = exc.PushResult.Errors;
                //}
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            //if (syncErrors != null)
            //{
            //    foreach (var error in syncErrors)
            //    {
            //        if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            //        {
            //            //Update failed, reverting to server's copy.
            //            await error.CancelAndUpdateItemAsync(error.Result);
            //        }
            //        else
            //        {
            //            // Discard local change.
            //            await error.CancelAndDiscardItemAsync();
            //        }
            //    }
            //}
        }

        #endregion

        #region Utility

        /// <summary>
        /// Determines whether [is user valid].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is user valid]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserValid()
        {
            //if (client.CurrentUser == null)
            //{
            //    return false;
            //}
            //else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the azure BLOB connection string
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAzureBlobConnection()
        {
            string result = "";
            try
            {

                result = await projectServices.GetAzureConnectionAsync().ConfigureAwait(false);
                //result = output.Value<string>();
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
            }

            return result;
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetSettings()
        {
            string result = "";
            try
            {
                result = await projectServices.GetAppConfigAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }

            return result;
        }


        #endregion

        #region SyncLog

        /// <summary>
        /// Posts the synchronize log.
        /// </summary>
        /// <param name="SyncMode">The synchronize mode.</param>
        /// <param name="TransactionBatchId">The transaction batch identifier.</param>
        /// <returns></returns>
        public async Task PostSyncLog(string SyncMode, Guid TransactionBatchId)
        {
#if ANDROID
            string deviceId = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver
                                                                        , Android.Provider.Settings.Secure.AndroidId) ?? string.Empty;
#elif IOS
            string deviceId = UIKit.UIDevice.CurrentDevice.IdentifierForVendor?.ToString() ?? string.Empty;
#endif
            var myEntry = new SyncLogRequest
            {
                Time = DateTime.UtcNow,
                DeviceId = deviceId,
                Platform = DeviceInfo.Platform.ToString() + " (" + DeviceInfo.Current.Version + ")",
                Model = DeviceInfo.Current.Model,
                SyncMode = SyncMode,
                TransactionBatchId = TransactionBatchId
            };

            var a = JsonConvert.SerializeObject(myEntry);


            try
            {
                // SyncLog/PostLogItem
                await projectServices.PostSyncLogAsync(myEntry).ConfigureAwait(false);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.StackTrace}");
            }
        }

        #endregion
    }
}
