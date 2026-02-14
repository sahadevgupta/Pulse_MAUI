namespace Pulse_MAUI.Interfaces
{
    public interface IAppWorkflowManager
    {
        IBlobStorageService BlobStorageService { get; }
        IEngineerService EngineerService { get; }
        IItemService ItemService { get; }
        ILookupService LookupService { get; }
        IPunchService PunchService { get; }
        ISynchroniseService SynchroniseService { get; }
        ISyncLogService SyncLogService { get; }
        IUserService UserService { get; }
    }
}