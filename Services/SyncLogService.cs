using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class SyncLogService(IDataManager dataManager) : ISyncLogService
    {
        public async Task PostSyncLogStart(Guid TransactionBatchId)
        {
            await dataManager.PostSyncLog("Start", TransactionBatchId);
        }

        /// <summary>
        /// Posts the synchronize log finish.
        /// </summary>
        /// <returns></returns>
        public async Task PostSyncLogFinish(Guid TransactionBatchId)
        {
            await dataManager.PostSyncLog("Finish", TransactionBatchId);
        }
    }
}
