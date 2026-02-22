namespace Pulse_MAUI.Services
{
    public class SyncService(IPullServices pullServices) : ISyncService
    {

        public async Task SyncAsync()
        {
            var databaseTasks = new List<Task>();
            // 1. PUSH pending local changes
            //await PushPendingAsync();

            // 2. PULL server items
            var activityTask = pullServices.GetActivitiesFromServerToDBAsync();
            databaseTasks.Add(activityTask);
            // 3. APPLY to local DB
            //foreach (var item in serverItems)
            //    await _local.InsertLocalAsync(item);

            await Task.WhenAll(databaseTasks).ConfigureAwait(false);
        }

        // Push all queued offline operations
        //private async Task PushPendingAsync()
        //{
        //    var pending = await _local.GetPendingAsync();

        //    foreach (var op in pending)
        //    {
        //        try
        //        {
        //            var item = JsonSerializer.Deserialize<TodoItem>(op.PayloadJson);

        //            if (op.OperationType == "insert")
        //            {
        //                await _remote.InsertAsync(_table, item);
        //            }
        //            else if (op.OperationType == "update")
        //            {
        //                await _remote.UpdateAsync(_table, item, item.Id, item.Version);
        //            }
        //            else if (op.OperationType == "delete")
        //            {
        //                await _remote.DeleteAsync(_table, item.Id, item.Version);
        //            }

        //            // Remove after successful push
        //            await _local.DeletePendingAsync(op);
        //        }
        //        catch (Exception)
        //        {
        //            // Stop pushing — offline or conflict
        //            return;
        //        }
        //    }
        //}
    }
}
