using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class SynchroniseService(IBlobStorageService blobStorageService,
        IDataManager dataManager,
        IItemService itemService,
        ILookupService lookupService,
        IPunchService punchService) : ISynchroniseService
    {

        /// <summary>
        /// Downloads the BLOB data.
        /// </summary>
        /// <returns></returns>
        public async Task DownloadBlobData(string ConnectionString)
        {
            // get any blob images from the blob storage
            await GetBlobDataItems(ConnectionString);

        }

        /// <summary>
        /// Do a push and pull to the azure backend.
        /// </summary>
        /// <returns>The and pull data async.</returns>
        public async Task<List<string>> PushAndPullDataAsync(bool incremental, bool secondPass)
        {
            List<string> result = new List<string>();

            result = await dataManager.SyncPushAndPullItemsAsync(incremental, secondPass);

            return result;

        }

        /// <summary>
        /// Uploads the BLOB data.
        /// </summary>
        /// <returns></returns>
        public async Task UploadBlobData(string ConnectionString)
        {
            var controlTypes = await lookupService.GetControlTypeLookups();

            var ActivityControl = controlTypes.FirstOrDefault(c => c.Value == "Activity");
            if (ActivityControl != null)
            {
                int ActivityControlValue = ActivityControl.LookupId;
                // Upload any new blob items here!
                await PutBlobDataItems(ActivityControlValue, ConnectionString);
            }

            // Reconsile new Punch Item -> Item records!
            await ReconsilePunchItems();

            var PunchControl = controlTypes.FirstOrDefault(c => c.Value == "Punch");
            if (PunchControl != null)
            {
                int PunchControlValue = PunchControl.LookupId;
                // Upload any new blob items here!
                await PutBlobDataItems(PunchControlValue, ConnectionString);
            }
        }

        /// <summary>
        /// Puts the BLOB data items.
        /// </summary>
        /// <param name="ControlType">Type of the control.</param>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
        private async Task PutBlobDataItems(int ControlType, string ConnectionString)
        {
            // clear the local data storage

            //BlobStorageService.Instance.BlobConnectionString = ConnectionString;

             blobStorageService.ClearLocalStorage();

            var itemData = await itemService.GetItemListAsync();


            if (itemData != null)
            {

                var newItems = itemData.Where(i => i.AzurePath == null && i.LocalPath != null && i.ControlType == ControlType);

                foreach (Models.Item item in newItems)
                {

                    // only upload records with non-null RecordID's
                    if (item.RecordID != null)
                    {
                        await blobStorageService.PushLocalToBlob(item);
                    }
                    else
                    {

                    }

                }

            }
        }

        /// <summary>
        /// Reconsiles the punch items (adds missing Root Punch Id for those images attached to a new punch item)
        /// </summary>
        /// <returns></returns>
        public async Task ReconsilePunchItems()
        {
            IEnumerable<PunchItem> punches = await punchService.GetPunchListAsync();
            IEnumerable<Item> items = await itemService.GetItemListAsync();

            if (punches.Count() > 0)
            {
                if (items != null)
                {

                    var nullItems = items.Where(i => i.RecordID == null && i.LocalReferenceID != null);

                    foreach (Item item in nullItems)
                    {

                        var PunchItem = punches.FirstOrDefault(p => p.MobileId == item.LocalReferenceID);

                        if (PunchItem != null)
                        {
                            item.RecordID = PunchItem.PunchId;
                            await itemService.SaveItem(item);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Gets the BLOB data items.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <returns></returns>
        private async Task GetBlobDataItems(string ConnectionString)
        {
            // clear the local data storage
            blobStorageService.ClearLocalStorage();

            var itemData = await itemService
            .GetItemListAsync();

            //itemData.FirstOrDefault(x => x.)


            foreach (Models.Item item in itemData)
            {
                await blobStorageService.RetrieveBlobToLocal(item);
            }

        }
    }
}
