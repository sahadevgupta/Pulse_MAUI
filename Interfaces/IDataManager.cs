using Microsoft.Datasync.Client;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces
{
    public interface IDataManager
    {
        DatasyncClient CurrentClient { get; set; }

        Task DeleteItemAsync(Item item);
        Task<Activity> GetActivityById(string id);
        Task<ActivityTask> GetActivityTaskById(string id);
        Task<IEnumerable<Activity>> GetAllActivitiesAsync();
        Task<IEnumerable<ActivityTask>> GetAllActivityTasksForActivityAsync(Activity activity);
        Task<IEnumerable<CommissioningSystem>> GetAllCommissioningSystemsAsync();
        Task<IEnumerable<Component>> GetAllComponentsAsync();
        Task<IEnumerable<Discipline>> GetAllDisciplines();
        Task<IEnumerable<Engineer>> GetAllEngineersAsync();
        Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<IEnumerable<Lookup>> GetAllLookupsAsync();
        Task<IEnumerable<Priority>> GetAllPriority();
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<IEnumerable<PunchItem>> GetAllPunchItemsAsync();
        Task<IEnumerable<Unit>> GetAllUnitsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<string> GetAzureBlobConnection();
        Task<PunchItem> GetPunchItemById(string id);
        Task<string> GetSettings();
        Task InitDataManager();
        void InitDataManager(string url);
        bool IsUserValid();
        Task<MobileServiceUser> LoginAsync(string azureMobileServiceUrl);
        Task? LogoutAsync();
        Task PostSyncLog(string SyncMode, Guid TransactionBatchId);
        Task SaveActivityAsync(Activity activity);
        Task SaveActivityTaskAsync(ActivityTask activityTask);
        Task SaveItemAsync(Item item);
        Task SavePunchItemAsync(PunchItem punchItem);
        Task<List<string>> SyncPushAndPullItemsAsync(bool incremental, bool secondPass);
        Task SyncPushAndPurgeAsync();
    }
}