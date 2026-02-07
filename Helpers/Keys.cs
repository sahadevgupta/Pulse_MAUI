
using Pulse_MAUI.Interfaces;

namespace Pulse_MAUI.Helpers
{
    /// <summary>
    /// Class with all PCA specific keys to use in the application. 
    /// Fogbugz Case:
    /// Author: Manuel Dambrine
    /// Created: 29/03/2013
    /// </summary>
    public class Keys
    {

        /// <summary>
        /// The azure service endpoint.
        /// </summary>
        /// <value>
        /// The azure service.
        /// </value>
        public static string AzureService
        {
            get
            {
                //"https://pcamobile.azurewebsites.net"
                return Preferences.Get("ServiceURL", "https://www.syncservice.com");
            }
        }


        /// <summary>
        /// Gets a value indicating whether [use location services].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use location services]; otherwise, <c>false</c>.
        /// </value>
        public static bool UseLocationServices
        {
            get
            {
                ISettingProvider settingProvider = IPlatformApplication.Current?.Services.GetService<ISettingProvider>()!;
                return settingProvider.UseLocationServices();
            }
        }

        /// <summary>
        /// Gets the BLOB storage.
        /// </summary>
        /// <value>
        /// The BLOB storage.
        /// </value>
        public static string BlobStorage
        {
            get
            {
                return Preferences.Get("StorageNAME", string.Empty);
            }
        }


        public static string AppTitle
        {
            get
            {
                return Preferences.Get("AppTitle", "Pulse");
            }
        }


    }
}
