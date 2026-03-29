using System;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IMediaService
{
    Task<ImageFile> PickPhotoAsync();
    Task<ImageFile> TakePhoto();
}
