using System;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services;

public class MediaService : IMediaService
{
    public async Task<ImageFile> PickPhotoAsync()
    {
        ImageFile image = new ImageFile();
        var results = await MediaPicker.PickPhotosAsync(new MediaPickerOptions
        {

            // Optional processing for images
            MaximumWidth = 1024,
            MaximumHeight = 768,
            CompressionQuality = 85,
            RotateImage = true,
            PreserveMetaData = true,
        });

        if (results != null && results.Any())
        {
            var localFilePath = await SaveFileToCacheAsync(results[0]);
            image.Url = localFilePath;
            image.AvailableToDelete = true;
        }

        return image;
    }

    public async Task<ImageFile> TakePhoto()
    {
        string localFilePath = string.Empty;
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult? photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
            {
                CompressionQuality = 50,
            });

            if (photo != null)
            {
                localFilePath = await SaveFileToCacheAsync(photo);
            }


        }
        ImageFile image = new ImageFile();
        image.Url = localFilePath;
        image.AvailableToDelete = true;
        return image;
    }

    private static async Task<string> SaveFileToCacheAsync(FileResult photo)
    {
        string localFilePath;
        var folderPath = Path.Combine(FileSystem.CacheDirectory, "Captured");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        // save the file into local storage
        localFilePath = Path.Combine(folderPath, photo.FileName);

        using Stream sourceStream = await photo.OpenReadAsync();
        using FileStream localFileStream = File.OpenWrite(localFilePath);

        await sourceStream.CopyToAsync(localFileStream);
        return localFilePath;
    }
}
