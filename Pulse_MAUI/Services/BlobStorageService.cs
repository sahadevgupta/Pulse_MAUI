using System.Diagnostics;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using Pulse_MAUI.Constants;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services
{
    public class BlobStorageService(IDataManager dataManager, ILookupService lookupService, IItemService itemService) : IBlobStorageService
    {
        /// <summary>
        /// Removes all the data (images etc) stored in local storage
        /// </summary>
        public void ClearLocalStorage()
        {
            try
            {
                string blobStoragePath = $"{AppConstants.AppRootFolder}/{AppHelpers.BlobStorageName}";
                var doesExist = Directory.Exists(blobStoragePath);
                if (!doesExist)
                {
                    Directory.CreateDirectory($"{AppConstants.AppRootFolder}/{AppHelpers.BlobStorageName}");
                }
                else
                {
                    Directory.Delete(blobStoragePath, recursive: true);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ClearLocalStorage :" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Retrieves the blob data from Azure and stores in same folder structure locally />
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public async Task RetrieveBlobToLocal(Item item)
        {

            try
            {
                string BlobPath = item.AzurePath;

                if (BlobPath.Contains("\\"))
                {
                    BlobPath = BlobPath.Replace("\\", "/");
                }

                var cloudBlobContainer = await GetBlobContainer();
                CloudBlob blob = cloudBlobContainer.GetBlobReference(BlobPath);

                // get the blob attributes & check it has a length

                if (BlobExists(blob))
                {
                    await blob.FetchAttributesAsync();


                    if (blob.Properties.Length > 0)
                    {
                        byte[] content = new byte[blob.Properties.Length];
                        await blob.DownloadToByteArrayAsync(content, 0);

                        if (content != null)
                        {
                            // root app sandbox folder
                            string rootPath = FileSystem.AppDataDirectory;

                            // extract folder hierarchy
                            IList<string> folders = Helpers.FileUtility.GetFoldersFromPath(BlobPath);
                            string fileName = Helpers.FileUtility.GetFileName(BlobPath);

                            // build directory path
                            string folderPath = rootPath;

                            foreach (string folder in folders)
                            {
                                folderPath = Path.Combine(folderPath, folder);
                            }

                            // create directory (safe, recursive)
                            Directory.CreateDirectory(folderPath);

                            // final file path (CORRECT!)
                            string finalFilePath = Path.Combine(folderPath, fileName);

                            // write file
                            File.WriteAllBytes(finalFilePath, content);
                        }
                    }

                    // if (blob.Properties.Length > -1)
                    // {
                    //     // write the blob contents to a byte array
                    //     byte[] content = new byte[blob.Properties.Length];
                    //     await blob.DownloadToByteArrayAsync(content, 0);

                    //     if (!Object.Equals(content, null))
                    //     {
                    //         // Write the bytes to a 'sandboxed' file on the local device

                    //         var folderPath = Path.Combine(AppConstants.AppRootFolder, AppHelpers.BlobStorageName);

                    //         if (!Directory.Exists(folderPath))
                    //             Directory.CreateDirectory(folderPath);


                    //         string fileName = Helpers.FileUtility.GetFileName(BlobPath);
                    //         IList<string> folders = Helpers.FileUtility.GetFoldersFromPath(BlobPath);

                    //         foreach (string folder in folders)
                    //         {
                    //             if (folder.Length == 0)
                    //             {
                    //                 //throw new Exception("Invalid Folder Path");
                    //                 return;
                    //             }
                    //         }


                    //         // build the folder hierarchy
                    //         var rootPath = FileSystem.AppDataDirectory;
                    //         var currentPath = rootPath;

                    //         foreach (string folder in folders)
                    //         {
                    //             currentPath = Path.Combine(currentPath, folder);

                    //             if (!Directory.Exists(currentPath))
                    //                 Directory.CreateDirectory(currentPath);
                    //         }


                    //         // store the file at the end of the hierarchy

                    //         var currentFilePath = rootPath;

                    //         foreach (string folder in folders)
                    //         {
                    //             currentPath = Path.Combine(currentFilePath, folder);

                    //             if (!File.Exists(currentPath))
                    //                 File.WriteAllBytes(currentPath, content);
                    //         }

                    //     }
                    // }

                }
                else
                {
                    // Blob doesnt exist?

                }
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();
            }
        }

        /// <summary>
        /// Pushes the local items to BLOB.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public async Task PushLocalToBlob(Item item)
        {
            var controlTypes = await lookupService.GetControlTypeLookups();
            int ActivityValue = controlTypes.FirstOrDefault(c => c.Value == "Activity")?.LookupId ?? 0;
            int PunchValue = controlTypes.FirstOrDefault(c => c.Value == "Punch")?.LookupId ?? 0;

            string blobStorageRef = AppHelpers.BlobStorageName;
            string filename = Helpers.FileUtility.GetFileName(item.LocalPath);

            string blobPath = "Storage_Projects" + @"\" + "Project_" + item.ProjectId.ToString();

            // setup a base record Id
            string RecordId = "";

            if (item.RecordId != null)
            {
                RecordId = item.RecordId.ToString();
            }

            if (RecordId.Length > 0)
            {
                if (item.ControlType == ActivityValue)
                {
                    blobPath = blobPath + @"\Activity\" + RecordId + @"\" + filename;
                }

                if (item.ControlType == PunchValue)
                {
                    blobPath = blobPath + @"\Punch\" + RecordId + @"\" + filename;
                }

                if (item.ControlType != null)
                {
                    var cloudBlobContainer = await GetBlobContainer();
                    CloudBlob blob = cloudBlobContainer.GetBlobReference(blobPath);

                    var imgByte = System.IO.File.ReadAllBytes(item.LocalPath);

                    if (imgByte != null)
                    {
                        blob.Properties.ContentType = item.MimeType;
                        CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(blobPath);
                        await blockBlob.UploadFromByteArrayAsync(imgByte, 0, imgByte.Length);

                        item.AzurePath = blobPath;
                        item.LocalPath = null;
                        item.Size = imgByte.Length;

                        await itemService.SaveItem(item);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the BLOB container.
        /// </summary>
        /// <returns></returns>
        private async Task<CloudBlobContainer> GetBlobContainer()
        {
            var client = await GetAzureClient();
            return client.GetContainerReference(AppHelpers.BlobStorageName);
        }

        /// <summary>
        /// Gets the azure client.
        /// </summary>
        /// <returns></returns>
        private async Task<CloudBlobClient> GetAzureClient()
        {
            string blobConnectionString = await dataManager.GetAzureBlobConnection();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(blobConnectionString);

            return storageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Check if BLOB exists
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        private bool BlobExists(CloudBlob blob)
        {
            try
            {
                blob.FetchAttributesAsync();
                return true;
            }
            catch
            {
                return false;

            }
        }
    }
}
