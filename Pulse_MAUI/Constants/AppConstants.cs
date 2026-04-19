using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Constants
{
    public static class AppConstants
    {
        public static string AppRootFolder => FileSystem.Current.AppDataDirectory;
        public static string CapturedPhotoFolder => FileSystem.Current.AppDataDirectory;
    }
}
