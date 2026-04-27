using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.Serialization;
using Microsoft.Datasync.Client.SQLiteStore;

using Newtonsoft.Json;

using Pulse_MAUI.Constants;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Models.Request;
using Pulse_MAUI.Models.Response;
using Activity = Pulse_MAUI.Models.Activity;

namespace Pulse_MAUI.Data
{
    public partial class DataManager : IDataManager
    {
        private readonly IServiceProvider _serviceProvider;
        IProjectServices projectServices => _serviceProvider.GetRequiredService<IProjectServices>();
        private bool isInitialized = false;

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

        public DataManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await InitDataManager();

            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PCATablet.Core.Data.DataManager"/> class.
        /// </summary>
        public async Task InitDataManager()
        {
            if (isInitialized || AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
                return;

            isInitialized = false;

            var dbPath = Path.Combine(AppConstants.AppRootFolder, DBConstants.DatabaseFilename);
            Debug.WriteLine($"Database path: {dbPath}");
            var sqliteUri = $"file:{dbPath}";
            localStore = new OfflineSQLiteStore(sqliteUri);
            Debug.WriteLine("OfflineSQLiteStore created successfully");

            // setup the local store for each of the DataTables
            Debug.WriteLine("Defining tables in local store...");
            localStore.DefineTable<Activity>();
            localStore.DefineTable<PunchItem>();
            localStore.DefineTable<ActivityTask>();
            localStore.DefineTable<Component>();
            localStore.DefineTable<Lookup>();
            localStore.DefineTable<CommissioningSystem>();
            localStore.DefineTable<Project>();
            localStore.DefineTable<Unit>();
            localStore.DefineTable<Discipline>();
            localStore.DefineTable<User>();
            localStore.DefineTable<Item>();
            localStore.DefineTable<Equipment>();
            localStore.DefineTable<Priority>();
            localStore.DefineTable<Engineer>();
            Debug.WriteLine("All tables defined successfully");

            // Configure options
            var options = new DatasyncClientOptions
            {
                HttpPipeline = new HttpMessageHandler[]
                {

                    new AuthHeaderHandler(),
                },
                SerializerSettings = new DatasyncSerializerSettings
                {
                    // CamelCasePropertyNames = true,  // Commented out - models use explicit JsonProperty attributes
                },
                OfflineStore = localStore
            };

            //Create client with options
            client = new DatasyncClient(AppHelpers.AzureServiceUrl, options);

            // *** FIX: MUST AWAIT THIS ***
            Debug.WriteLine("Initializing offline store...");
            try
            {
                await client.InitializeOfflineStoreAsync();
                Debug.WriteLine("Offline store initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Offline store initialization failed: {ex.Message}");
                throw;
            }

            // Return each Table type
            this.activityTable = client.GetOfflineTable<Activity>();
            Console.WriteLine("Data sync client initialixed : " + activityTable.GetType().FullName);
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

            isInitialized = true;

        }

        private async Task<AuthenticationToken> TokenRequestor()
        {
            AuthResultDto authResultDto = new();
            var jsonResult = await SecureStorage.GetAsync(ADConstants.AuthResultKey);

            if (!string.IsNullOrWhiteSpace(jsonResult))
            {
                authResultDto = JsonConvert.DeserializeObject<AuthResultDto>(jsonResult)!;
            }

            return new AuthenticationToken
            {
                DisplayName = authResultDto.DisplayName,
                UserId = authResultDto.UserId,
                Token = authResultDto.AccessToken,
                ExpiresOn = authResultDto.ExpiresOn.GetValueOrDefault()
            };
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
            await InitDataManager();
            return await disciplineTable.ToListAsync();
        }

        /// <summary>
        /// Gets all activities async.
        /// </summary>
        /// <returns>All activities async.</returns>
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            await InitDataManager();
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
            await InitDataManager();
            return await activityTaskTable
                .Where(p => p.ActivityId == activity.PCAId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the activity task by identifier.
        /// </summary>
        /// <returns>The activity task by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<ActivityTask> GetActivityTaskById(string id)
        {
            await InitDataManager();
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
            await InitDataManager();
            var activity = await activityTable
                .Where(p => p.Id == id)
                .ToListAsync();

            return activity.FirstOrDefault();
        }

        /// <summary>
        /// Gets all punch items async.
        /// </summary>
        /// <returns>All punch items async.</returns>
        public async Task<IEnumerable<PunchItem>> GetAllPunchItemsAsync()
        {
            await InitDataManager();
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
            await InitDataManager();
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
            await InitDataManager();
            return await engineerTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all users async.
        /// </summary>
        /// <returns>All Users async.</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            await InitDataManager();
            return await userTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all projects async.
        /// </summary>
        /// <returns>The all projects async.</returns>
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            await InitDataManager();
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
            await InitDataManager();
            return await unitTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all commissioning systems async.
        /// </summary>
        /// <returns>All commissioning systems async.</returns>
        public async Task<IEnumerable<CommissioningSystem>> GetAllCommissioningSystemsAsync()
        {
            await InitDataManager();
            return await commissioningSystemTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all components async.
        /// </summary>
        /// <returns>All components async.</returns>
        public async Task<IEnumerable<Component>> GetAllComponentsAsync()
        {
            await InitDataManager();
            return await componentTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all lookups async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync()
        {
            await InitDataManager();
            return await lookupTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all items async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            await InitDataManager();
            return await itemTable
                .ToListAsync();
        }

        /// <summary>
        /// Gets all equipment asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            await InitDataManager();
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
            await InitDataManager();
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
            await InitDataManager();
            await activityTaskTable.ReplaceItemAsync(activityTask);
        }

        /// <summary>
        /// Saves the activity asynchronous.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        public async Task SaveActivityAsync(Activity activity)
        {
            await InitDataManager();
            await activityTable.ReplaceItemAsync(activity);
        }

        public async Task SavePunchItemAsync(PunchItem punchItem)
        {
            await InitDataManager();
            Debug.WriteLine($"Saving punch item. Table initialized: {punchItemTable != null}, Item ID: {punchItem.Id}");
            if (string.IsNullOrEmpty(punchItem.Id))
            {
                Debug.WriteLine("Inserting new punch item...");
                await punchItemTable.InsertItemAsync(punchItem);
                Debug.WriteLine("Punch item inserted successfully");
            }
            else
            {
                Debug.WriteLine("Updating existing punch item...");
                await punchItemTable.ReplaceItemAsync(punchItem);
                Debug.WriteLine("Punch item updated successfully");
            }
        }

        /// <summary>
		/// Saves the Item async.
		/// </summary>
		/// <returns>async task.</returns>
		/// <param name="Item">Item to save.</param>
        public async Task SaveItemAsync(Item item)
        {
            await InitDataManager();
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
                await itemTable.ReplaceItemAsync(item);
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
            await InitDataManager();
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
            await InitDataManager();
            Debug.WriteLine($"Starting sync - Incremental: {incremental}, SecondPass: {secondPass}, Client initialized: {client != null}");
            //ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            List<string> Errors = new List<string>();

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet || Connectivity.Current.NetworkAccess == NetworkAccess.ConstrainedInternet)
            {

                try
                {

                    //First do a push
                    long? _pendingOperations = this.client.PendingOperations;
                    Debug.WriteLine($"Pending operations before push: {_pendingOperations}");

                    if (_pendingOperations > 0)
                    {
                        Debug.WriteLine("Executing push operation...");
                        try
                        {
                            await this.client.PushTablesAsync();
                            Debug.WriteLine("Push completed successfully");
                        }
                        catch (Exception pushEx)
                        {
                            Debug.WriteLine($"Push failed: {pushEx.Message}");
                            Errors.Add($"Push failed: {pushEx.Message}");
                            // Continue with pull even if push fails
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No pending operations to push");
                    }

                    var a1 = await this.itemTable.ToListAsync();
                    await this.itemTable.PullItemsAsync(this.itemTable.CreateQuery());

                    var a = await this.itemTable.ToListAsync();


                    if (!secondPass)
                    {
                        await this.activityTable.PurgeItemsAsync(activityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.activityTable.PullItemsAsync(this.activityTable.CreateQuery());

                        await this.punchItemTable.PurgeItemsAsync(punchItemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.punchItemTable.PullItemsAsync(this.punchItemTable.CreateQuery());

                        await this.engineerTable.PurgeItemsAsync(engineerTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.engineerTable.PullItemsAsync(this.engineerTable.CreateQuery());

                        await this.userTable.PurgeItemsAsync(userTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.userTable.PullItemsAsync(this.userTable.CreateQuery());

                        await this.projectTable.PurgeItemsAsync(projectTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.projectTable.PullItemsAsync(this.projectTable.CreateQuery());

                        await this.commissioningSystemTable.PurgeItemsAsync(commissioningSystemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.commissioningSystemTable.PullItemsAsync(this.commissioningSystemTable.CreateQuery());

                        await this.unitTable.PurgeItemsAsync(unitTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.unitTable.PullItemsAsync(this.unitTable.CreateQuery());

                        await this.componentTable.PurgeItemsAsync(componentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.componentTable.PullItemsAsync(this.componentTable.CreateQuery());


                        await this.lookupTable.PullItemsAsync(this.lookupTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PullOptions
                        {
                            QueryId = incremental ? "LookupDataIncremental" : null
                        });


                        await this.priorityTable.PurgeItemsAsync(priorityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.priorityTable.PullItemsAsync(this.priorityTable.CreateQuery());

                        await this.activityTaskTable.PurgeItemsAsync(activityTaskTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.activityTaskTable.PullItemsAsync(this.activityTaskTable.CreateQuery());

                        await this.equipmentTable.PurgeItemsAsync(equipmentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.equipmentTable.PullItemsAsync(this.equipmentTable.CreateQuery());


                        await this.disciplineTable.PurgeItemsAsync(disciplineTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                        await this.disciplineTable.PullItemsAsync(this.disciplineTable.CreateQuery());

                    }

                }
                catch (DatasyncConflictException conflict)
                {
                    var response = conflict.Request;

                    if (response != null && response.Content != null)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(content);

                        Errors.Add("(PushFailedException) due to conflicts" + content);
                    }

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
                    if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                    {
                        Errors.Add("Error on Sync: Unable to connect to the server.\nIt appears there is an internet issue.Please check your connection and try syncing again.");

                    }
                    else if (ex.Message.Contains("Unable to resolve host"))
                    {
                        Errors.Add("Error on Sync: Unable to connect to the server.\nIt appears there is an internet issue.Please check your connection and try syncing again.");
                    }
                    else
                    {
                        Errors.Add("Error on Sync (Exception) " + ex.ToString());
                    }

                }
            }
            else
            {
                Errors.Add("Error on Sync: Unable to connect to the server.\nIt appears there is an internet issue.Please check your connection and try syncing again.");
            }
            return Errors;
        }

        /// <summary>
        /// Pushes the data to azure and purges the data locally
        /// </summary>
        /// <returns>async task.</returns>
        public async Task SyncPushAndPurgeAsync()
        {

            await InitDataManager();

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
                SentrySdk.CaptureException(exc);
            }
            catch (Exception exception)
            {
                SentrySdk.CaptureException(exception);
            }
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

                result = await projectServices.GetAzureConnectionAsync();
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
                result = await projectServices.GetAppConfigAsync();
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
                await projectServices.PostSyncLogAsync(myEntry);
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.StackTrace}");
            }
        }

        #endregion
    }
}