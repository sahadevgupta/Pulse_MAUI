using System;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services;



public class FileService(IDataManager dataManager,
    ILookupService lookupService) : IFileService
{
    /// <summary>
    /// Uploads the BLOB images.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <returns></returns>
    public async Task UploadBlobImages(Models.Activity activity)
    {


        var awaitingFiles = new List<ImageFile>();
        var blobStorageRef = AppHelpers.BlobStorageName;
        var availableItems = await dataManager.GetAllItemsAsync();
        var controlTypes = await lookupService.GetControlTypeLookups();
        int ActivityControlValue = controlTypes.FirstOrDefault(c => c.Value == "Activity").LookupId;
        var awaitingItemsForActivity = availableItems.Where(i => i.AzurePath == null && i.LocalPath != null && i.ControlType == ActivityControlValue);

    }

    /// <summary>
    /// Uploads the BLOB images.
    /// </summary>
    /// <param name="punch">The punch.</param>
    /// <returns></returns>
    public async Task UploadBlobImages(Models.PunchItem punch)
    {
        var awaitingFiles = new List<ImageFile>();
        var blobStorageRef = AppHelpers.BlobStorageName;
        var availableItems = await dataManager.GetAllItemsAsync();
        var controlTypes = await lookupService.GetControlTypeLookups();
        int PunchControlValue = controlTypes.FirstOrDefault(c => c.Value == "Punch").LookupId;
        var awaitingItemsForPunch = availableItems.Where(i => i.AzurePath == null && i.LocalPath != null && i.ControlType == PunchControlValue);

    }

    /// <summary>
    /// Fetches the item files.
    /// </summary>
    /// <param name="punchItem">The punch item.</param>
    /// <returns></returns>
    public async Task<IEnumerable<ImageFile>> FetchItemFiles(Models.PunchItem punchItem)
    {

        if (punchItem == null)
        {
            Enumerable.Empty<ImageFile>();
        }


        var availableFiles = new List<ImageFile>();

        var blobStorageRef = AppHelpers.BlobStorageName;

        var controlTypes = await lookupService.GetControlTypeLookups();
        int PunchControlValue = controlTypes.FirstOrDefault(c => c.Value == "Punch").LookupId;


        if (punchItem.PunchId == null)
        {
            var availableItems = await dataManager.GetAllItemsAsync();
            var availableItemsForPunch = availableItems.Where(i => i.LocalReferenceID == punchItem.MobileId && i.ControlType == PunchControlValue && i.LocalReferenceID != null);

            // for each item available add the image ref and correct the path
            foreach (Item item in availableItemsForPunch)
            {
                ImageFile img = new ImageFile();
                img.ChecklistStep = item.CheckListStep;
                img.Description = item.Description;
                img.ControlType = item.ControlType;


                if (!String.IsNullOrEmpty(item.AzurePath))
                {
                    img.Url = FileSystem.Current.AppDataDirectory + "/" + blobStorageRef + "/" + item.AzurePath.Replace("\\", "/");
                    img.AvailableToDelete = false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(item.LocalPath))
                    {
                        img.Url = item.LocalPath;
                        img.AvailableToDelete = true;
                    }
                }
                availableFiles.Add(img);
            }
        }
        else
        {
            var availableItems = await dataManager.GetAllItemsAsync();
            var availableItemsForPunch = availableItems.Where(i => i.RecordId == punchItem.PunchId && i.ControlType == PunchControlValue);


            // for each item available add the image ref and correct the path
            foreach (Item item in availableItemsForPunch)
            {
                ImageFile img = new ImageFile();
                img.ChecklistStep = item.CheckListStep;
                img.Description = item.Description;
                img.ControlType = item.ControlType;


                if (!String.IsNullOrEmpty(item.AzurePath))
                {
                    string LocalAzureReference = item.AzurePath.Replace("\\", "/");
                    img.Url = FileSystem.Current.AppDataDirectory + "/" + blobStorageRef + "/" + LocalAzureReference;
                    img.AvailableToDelete = false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(item.LocalPath))
                    {
                        img.Url = item.LocalPath;
                        img.AvailableToDelete = true;
                    }
                }
                availableFiles.Add(img);
            }
        }
        return availableFiles;
    }

    /// <summary>
    /// Fetch items for a specific record Id
    /// </summary>
    public async Task<IEnumerable<ImageFile>> FetchItemFiles(Models.Activity activity)
    {

        if (activity == null)
        {
            Enumerable.Empty<ImageFile>();
        }


        var availableFiles = new List<ImageFile>();

        var blobStorageRef = AppHelpers.BlobStorageName;

        var controlTypes = await lookupService.GetControlTypeLookups();
        int ActivityControlValue = controlTypes.FirstOrDefault(c => c.Value == "Activity").LookupId;

        var availableItems = await dataManager.GetAllItemsAsync();
        var availableItemsForActivity = availableItems.Where(i => i.RecordId == activity.PCAId && i.ControlType == ActivityControlValue);

        // for each item available add the image ref and correct the path
        foreach (Item item in availableItemsForActivity)
        {
            ImageFile img = new ImageFile();
            img.ChecklistStep = item.CheckListStep;
            img.Description = item.Description;
            img.ControlType = item.ControlType;


            if (!String.IsNullOrEmpty(item.AzurePath))
            {
                img.Url = FileSystem.Current.AppDataDirectory + "/" + blobStorageRef + "/" + item.AzurePath.Replace("\\", "/");
                img.AvailableToDelete = false;
            }
            else
            {
                if (!String.IsNullOrEmpty(item.LocalPath))
                {
                    img.Url = item.LocalPath;
                    img.AvailableToDelete = true;
                }
            }
            availableFiles.Add(img);
        }
        return availableFiles;

    }

    public async Task<bool> DeleteImageAsync(ImageFile imageFile, int? recordId)
    {
        IEnumerable<Item> items = await dataManager.GetAllItemsAsync();
        Item? itemToDelete = items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordId == recordId);
        if (itemToDelete != null)
        {
            await dataManager.DeleteItemAsync(itemToDelete);
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateImageDescription(ImageFile imageFile, int? recordId, string description, string checklistStep)
    {
        IEnumerable<Item> items = await dataManager.GetAllItemsAsync();
        Item? itemToUpdate = items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordId == recordId);
        if (itemToUpdate != null)
        {
            // update the item in the file list.
            itemToUpdate.Description = description;

            if (checklistStep != "None")
            {
                itemToUpdate.CheckListStep = Convert.ToInt32(checklistStep);
            }
            else
            {
                itemToUpdate.CheckListStep = null;
            }
            await dataManager.SaveItemAsync(itemToUpdate);
            return true;
        }
        return false;
    }


    /*
     public async Task UpdateImageDescription(ActivityFileListViewModel fileListViewModel, int position, string description, string checklistStep)
     {
         var PCAId = fileListViewModel.Activity.PCAId;
         var imageFile = fileListViewModel.Files[position];

         await ItemService.Instance.FetchItemListForActivityAsync(PCAId);

         Item item = ItemService.Instance.Items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordID == PCAId);

         if (item != null)
         {

             // update the item in the file list.
             item.Description = description;
             fileListViewModel.Files[position].Description = description;

             if (checklistStep != "None")
             {
                 item.CheckListStep = Convert.ToInt32(checklistStep);
                 fileListViewModel.Files[position].ChecklistStep = Convert.ToInt32(checklistStep);
             }
             else
             {
                 item.CheckListStep = null;
                 fileListViewModel.Files[position].ChecklistStep = null;

             }

             await ItemService.Instance.SaveItem(item);


         }
     }



     /// <summary>
     /// Updates the image description.
     /// </summary>
     /// <param name="fileListViewModel">The file ListView model.</param>
     /// <param name="description">The description.</param>
     /// <param name="checklistStep">The checklist step.</param>
     /// <returns></returns>
     public async Task UpdateImageDescription(PunchFileListViewModel fileListViewModel, int position, string description, string checklistStep)
     {

         int? PunchId = fileListViewModel.PunchItem.PunchId;
         var imageFile = fileListViewModel.Files[position];

         await ItemService.Instance.FetchItemListForActivityAsync(PunchId);

         Item item = ItemService.Instance.Items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordID == PunchId);

         if (item != null)
         {

             // update the item in the file list.
             item.Description = description;
             fileListViewModel.Files[position].Description = description;

             if (checklistStep != "None")
             {
                 item.CheckListStep = Convert.ToInt32(checklistStep);
                 fileListViewModel.Files[position].ChecklistStep = Convert.ToInt32(checklistStep);
             }
             else
             {
                 item.CheckListStep = null;
                 fileListViewModel.Files[position].ChecklistStep = null;

             }

             await ItemService.Instance.SaveItem(item);


         }
     }


     /// <summary>
     /// Deletes the image.
     /// </summary>
     /// <param name="fileListViewModel">The file ListView model.</param>
     /// <returns></returns>
     public async Task<bool> DeleteImage(ActivityFileListViewModel fileListViewModel)
     {

         var PCAId = fileListViewModel.Activity.PCAId;
         if (fileListViewModel.Position <= ActivityFiles.Count())
         {
             var imageFile = fileListViewModel.Files[fileListViewModel.Position];

             IEnumerable<Item> items = await ItemService.Instance.GetItemListAsync();
             Item itemToDelete = items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordID == PCAId);

             // delete the item from the items table
             await ItemService.Instance.DeleteItem(itemToDelete);

             // delete the item from the local file instance
             ActivityFiles.Remove(imageFile);

             return true;

         }
         else
         {
             return false;
         }

     }


     /// <summary>
     /// Deletes the image.
     /// </summary>
     /// <param name="fileListViewModel">The file ListView model.</param>
     /// <returns></returns>
     public async Task DeleteImage(PunchFileListViewModel fileListViewModel)
     {

         var PunchId = fileListViewModel.PunchItem.PunchId;
         if (fileListViewModel.Position <= PunchFiles.Count())
         {
             var imageFile = fileListViewModel.Files[fileListViewModel.Position];

             IEnumerable<Item> items = await ItemService.Instance.GetItemListAsync();
             Item itemToDelete = items.FirstOrDefault(i => i.LocalPath == imageFile.Url && i.RecordID == PunchId);

             // delete the item from the items table
             await ItemService.Instance.DeleteItem(itemToDelete);

             // delete the item from the local file instance
             PunchFiles.Remove(imageFile);

         }

     }
 */
}
