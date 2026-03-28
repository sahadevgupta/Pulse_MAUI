using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Interfaces
{

    /// <summary>
    /// Interface with the contract for device implementation specific settings
    /// Fogbugz Case:
    /// Author: Manuel Dambrine
    /// Created: 29/03/2013
    /// </summary>
    public interface ISettingProvider
    {

        /// <summary>
        /// get the Azure service url
        /// </summary>
        /// <returns></returns>
        string AzureService();
        
        /// <summary>
        /// gets the BLOB storage name
        /// </summary>
        /// <returns></returns>
        string BlobStorage();

        /// <summary>
        /// Uses the location services.
        /// </summary>
        /// <returns></returns>
        bool UseLocationServices();
    }
}
