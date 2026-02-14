namespace Pulse_MAUI.Interfaces
{
    public interface ISyncLogService
    {
        Task PostSyncLogFinish(Guid TransactionBatchId);
        Task PostSyncLogStart(Guid TransactionBatchId);
    }
}