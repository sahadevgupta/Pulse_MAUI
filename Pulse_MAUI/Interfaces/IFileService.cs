using System;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IFileService
{
    Task<IEnumerable<ImageFile>> FetchItemFiles(Models.PunchItem punchItem);
    Task<IEnumerable<ImageFile>> FetchItemFiles(Models.Activity activity);
    Task UploadBlobImages(Activity activity);
    Task UploadBlobImages(PunchItem punch);
}