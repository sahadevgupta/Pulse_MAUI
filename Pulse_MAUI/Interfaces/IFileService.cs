using System;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IFileService
{
    Task<IEnumerable<ImageFile>> FetchItemFiles(Models.PunchItem punchItem);
    Task<IEnumerable<ImageFile>> FetchItemFiles(Models.Activity activity);
    Task UploadBlobImages(Activity activity);
    Task UploadBlobImages(PunchItem punch);
    Task<bool> DeleteImageAsync(ImageFile imageFile, int? recordId);
    Task<bool> UpdateImageDescription(ImageFile imageFile, int? recordId, string description, string checklistStep);
}