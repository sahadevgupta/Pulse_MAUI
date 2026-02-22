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
using Pulse_MAUI.Repository;
using Pulse_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Activity = Pulse_MAUI.Models.Activity;

namespace Pulse_MAUI.Data
{
    public partial class DataManager(IProjectServices projectServices,ISyncService syncService) : IDataManager
    {
        private readonly ILoginProvider? loginProvider = ServiceHelper.GetService<ILoginProvider>();

        #region Authentication
        /// <summary>
        /// Logs the user into the mobile client and server.
        /// </summary>
        /// <returns>async task.</returns>
        public async Task<MobileServiceUser> LoginAsync(string azureMobileServiceUrl)
        {
            return await loginProvider?.LoginAsync(azureMobileServiceUrl)!;
        }

        /// <summary>
        /// Logout from AD asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task? LogoutAsync()
        {
            //return loginProvider?.LogoutAsync(client);

            await Task.CompletedTask;
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets all disciplines.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Discipline>> GetAllDisciplines()
        {
            var disciplineRepo = AppHelpers.GetRepository<Discipline>();
            return await disciplineRepo.GetAllItemAsync();
        }

        /// <summary>
        /// Gets all activities async.
        /// </summary>
        /// <returns>All activities async.</returns>
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            var activityRepo = AppHelpers.GetRepository<Activity>();
            return await activityRepo.GetAllItemAsync();
        }

        /// <summary>
        /// Gets all activity tasks for activity async.
        /// </summary>
        /// <returns>All activity tasks.</returns>
        /// <param name="activity">Activity to get activity tasks for.</param>
        public async Task<IEnumerable<ActivityTask>> GetAllActivityTasksForActivityAsync(Activity activity)
        {
            var activityTaskRepo = AppHelpers.GetRepository<ActivityTask>();
            return await activityTaskRepo.GetFilteredItemAsync<ActivityTask>(p => p.ActivityId == activity.PCAId);
        }

        /// <summary>
        /// Gets the activity task by identifier.
        /// </summary>
        /// <returns>The activity task by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<ActivityTask> GetActivityTaskById(string id)
        {
            var activityTaskRepo = AppHelpers.GetRepository<ActivityTask>();

            var activityTask = await activityTaskRepo.GetFilteredItemAsync<ActivityTask>(p => p.Id == id);

            return activityTask.FirstOrDefault() ?? new();
        }


        /// <summary>
        /// Gets the activity by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Activity> GetActivityById(string id)
        {
            var activityRepo = AppHelpers.GetRepository<Activity>();

            var activity = await activityRepo.GetFilteredItemAsync<Activity>(p => p.Id == id);

            return activity.FirstOrDefault() ?? new();
        }

        /// <summary>
        /// Gets all punch items async.
        /// </summary>
        /// <returns>All punch items async.</returns>
        public async Task<IEnumerable<PunchItem>> GetAllPunchItemsAsync()
        {
            var punchItemRepo = AppHelpers.GetRepository<PunchItem>();

            return await punchItemRepo.GetAllItemAsync();
        }

        /// <summary>
        /// Gets the punch item by identifier.
        /// </summary>
        /// <returns>The punch item by identifier.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<PunchItem> GetPunchItemById(string id)
        {
            var punchItemRepo = AppHelpers.GetRepository<PunchItem>();

            var punchItem = await punchItemRepo.GetFilteredItemAsync<PunchItem>(p => p.Id == id);

            return punchItem.FirstOrDefault() ?? new();
        }

        /// <summary>
        /// Gets all engineers async.
        /// </summary>
        /// <returns>All Engineers async.</returns>
        public async Task<IEnumerable<Engineer>> GetAllEngineersAsync()
        {
            var engineerRepo = AppHelpers.GetRepository<Engineer>();

            return await engineerRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all users async.
        /// </summary>
        /// <returns>All Users async.</returns>
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var userRepo = AppHelpers.GetRepository<User>();
            return await userRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all projects async.
        /// </summary>
        /// <returns>The all projects async.</returns>
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projectRepo = AppHelpers.GetRepository<Project>();
            return await projectRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all units async.
        /// </summary>
        /// <returns>All units async.</returns>
        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            var unitRepo = AppHelpers.GetRepository<Unit>();
            return await unitRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all commissioning systems async.
        /// </summary>
        /// <returns>All commissioning systems async.</returns>
        public async Task<IEnumerable<CommissioningSystem>> GetAllCommissioningSystemsAsync()
        {
            var commissioningSystemRepo = AppHelpers.GetRepository<CommissioningSystem>();
            return await commissioningSystemRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all components async.
        /// </summary>
        /// <returns>All components async.</returns>
        public async Task<IEnumerable<Component>> GetAllComponentsAsync()
        {
            var componentRepo = AppHelpers.GetRepository<Component>();

            return await componentRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all lookups async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Lookup>> GetAllLookupsAsync()
        {
            var lookupRepo = AppHelpers.GetRepository<Lookup>();

            return await lookupRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all items async.
        /// </summary>
        /// <returns>All lookups async.</returns>
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var itempRepo = AppHelpers.GetRepository<Item>();

            return await itempRepo
                .GetAllItemAsync();
        }

        /// <summary>
        /// Gets all equipment asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            var equipmentRepo = AppHelpers.GetRepository<Equipment>();

            return await equipmentRepo
                 .GetAllItemAsync();

        }

        /// <summary>
        /// Gets all priority.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Priority>> GetAllPriority()
        {
            var priorityRepo = AppHelpers.GetRepository<Priority>();

            return await priorityRepo
                 .GetAllItemAsync();
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
            var activityTaskRepo = AppHelpers.GetRepository<ActivityTask>();

            await activityTaskRepo.InsertOrReplaceAsync(activityTask);
        }

        /// <summary>
        /// Saves the activity asynchronous.
        /// </summary>
        /// <param name="activity">The activity.</param>
        /// <returns></returns>
        public async Task SaveActivityAsync(Activity activity)
        {
            var activityRepo = AppHelpers.GetRepository<Activity>();

            await activityRepo.InsertOrReplaceAsync(activity);
        }

        /// <summary>
        /// Saves the punch item async.
        /// </summary>
        /// <returns>async task.</returns>
        /// <param name="punchItem">Punch item to save.</param>
        public async Task SavePunchItemAsync(PunchItem punchItem)
        {
            var punchItemRepo = AppHelpers.GetRepository<PunchItem>();
            await punchItemRepo.InsertOrReplaceAsync(punchItem);
        }

        /// <summary>
		/// Saves the Item async.
		/// </summary>
		/// <returns>async task.</returns>
		/// <param name="Item">Item to save.</param>
        public async Task SaveItemAsync(Item item)
        {

            var itemRepo = AppHelpers.GetRepository<Item>();
            await itemRepo.InsertOrReplaceAsync(item);
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
                var itemRepo = AppHelpers.GetRepository<Item>();
                await itemRepo.DeleteAsync(item);
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
                //long? _pendingOperations = this.client.PendingOperations;

                //await this.client.PushTablesAsync();


                //await this.itemTable.PurgeItemsAsync(null, null, CancellationToken.None);
    //            await  itemTable.PullItemsAsync(
    //query: itemTable.CreateQuery(),
    //cancellationToken: CancellationToken.None
//);

                if (!secondPass)
                {
                    await syncService.SyncAsync();
                    var activiRepo = AppHelpers.GetRepository<Activity>();
                    //await this.activityTable.PurgeItemsAsync(activityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.activityTable.PullItemsAsync(
                    //         this.activityTable.CreateQuery());

                    await Task.Delay(500);

                    var a =  await activiRepo.GetAllItemAsync();

                    //await this.punchItemTable.PurgeItemsAsync(punchItemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.punchItemTable.PullItemsAsync(
                    //    this.punchItemTable.CreateQuery());

                    //await this.engineerTable.PurgeItemsAsync(engineerTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.engineerTable.PullItemsAsync(
                    //    this.engineerTable.CreateQuery());

                    //await this.userTable.PurgeItemsAsync(userTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.userTable.PullItemsAsync(
                    //    this.userTable.CreateQuery());

                    //await this.projectTable.PurgeItemsAsync(projectTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.projectTable.PullItemsAsync(
                    //    this.projectTable.CreateQuery());

                    //await this.commissioningSystemTable.PurgeItemsAsync(commissioningSystemTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.commissioningSystemTable.PullItemsAsync(
                    //    this.commissioningSystemTable.CreateQuery());

                    //await this.unitTable.PurgeItemsAsync(unitTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.unitTable.PullItemsAsync(
                    //   this.unitTable.CreateQuery());

                    //await this.componentTable.PurgeItemsAsync(componentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.componentTable.PullItemsAsync(
                    //   this.componentTable.CreateQuery());

                    //await this.lookupTable.PullItemsAsync(this.lookupTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PullOptions
                    //{
                    //    QueryId = incremental ? "LookupDataIncremental" : null
                    //});

                    //await this.priorityTable.PurgeItemsAsync(priorityTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.priorityTable.PullItemsAsync(
                    //    this.priorityTable.CreateQuery());

                    //await this.activityTaskTable.PurgeItemsAsync(activityTaskTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.activityTaskTable.PullItemsAsync(
                    //    this.activityTaskTable.CreateQuery());

                    //await this.equipmentTable.PurgeItemsAsync(equipmentTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.equipmentTable.PullItemsAsync(
                    //    this.equipmentTable.CreateQuery());


                    //await this.disciplineTable.PurgeItemsAsync(disciplineTable.CreateQuery(), new Microsoft.Datasync.Client.Offline.PurgeOptions(), CancellationToken.None);
                    //await this.disciplineTable.PullItemsAsync(
                    //    this.disciplineTable.CreateQuery());

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
                //await this.client.PushTablesAsync();

                //await activityTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await punchItemTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await activityTaskTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await projectTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await commissioningSystemTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await unitTable.PurgeItemsAsync(null, null, CancellationToken.None);
                //await componentTable.PurgeItemsAsync(null, null, CancellationToken.None);
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
