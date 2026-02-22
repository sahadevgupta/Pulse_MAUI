using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCATablet.Core.Data;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System.Xml.Linq;

namespace Pulse_MAUI.ViewModels
{
    public partial class ImportSettingsPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        readonly IDataManager _dataManager;

        [ObservableProperty]
        private string? _url = "https://pulseargwebappmobile.azurewebsites.net";

        [ObservableProperty]
        private string? _serviceName;

        #endregion

        public ImportSettingsPageViewModel(IDataManager dataManager,
            IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _dataManager = dataManager;
        }

        #region [ Methods & Service Calls ]

        public ServiceInfo ReadSettingData(string fileContent)
        {
            // remove leading & trailing quotes
            fileContent = fileContent.Trim('"');

            // unescape inner quotes
            string xml = fileContent.Replace("\\\"", "\"");

            ServiceInfo serviceInfo = new ServiceInfo();
            serviceInfo.ServiceTitle = "";
            serviceInfo.ServiceURL = "";
            serviceInfo.StorageName = "";
            serviceInfo.ServiceError = "";


            if (fileContent.Length > 0)
            {
                try
                {
                    XDocument doc = XDocument.Parse(xml);

                    serviceInfo.ServiceTitle = doc.Descendants().FirstOrDefault(a => a.Name.LocalName == "SERVICETITLE")?.Value ?? string.Empty;
                    serviceInfo.ServiceURL = doc.Descendants().FirstOrDefault(a => a.Name.LocalName == "SERVICEURL")?.Value ?? string.Empty;
                    serviceInfo.StorageName = doc.Descendants().FirstOrDefault(a => a.Name.LocalName == "STORAGENAME")?.Value ?? string.Empty;

                }
                catch (Exception ex)
                {

                    serviceInfo.ServiceError = ex.Message;
                    var error = ex.Message;
                }
            }
            else
            {
                serviceInfo.ServiceError = "No Content in Response";
            }
            return serviceInfo;


        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task ImportSettings()
        {
            if (Url?.Length > 0 || Url != @"http://")
            {
                string customUrl = string.Empty;
                var lastCharacter = Url.Last();
                if (Url.EndsWith("/"))
                {
                    customUrl = Url + @"Mobile/ServiceSetting.xml";
                }
                else
                {
                    customUrl = Url + @"/Mobile/ServiceSetting.xml";
                }
                var user = await _dataManager.LoginAsync(Url);

                if (user is object)
                {
                    var setting = await _dataManager.GetSettings();
                    ServiceInfo info = ReadSettingData(setting);
                    var x = 1;

                    if (info.ServiceError.Length == 0)
                    {
                        AppHelpers.AppTitle = info.ServiceTitle;
                        AppHelpers.AzureServiceUrl = info.ServiceURL;
                        AppHelpers.BlobStorageName = info.StorageName;

                        ServiceName = "Found Service: " + info.ServiceTitle;
                        // Please restart the application
                    }
                    else
                    {
                        ServiceName = "No Service Found";
                    }
                }
                else
                {
                    ServiceName = "Unable to Authenticate User";
                }

            }
            else
            {
                ServiceName = "Please enter the provided address";
            }
        }

        [RelayCommand]
        private void Cancel()
        {
#if ANDROID
            Java.Lang.JavaSystem.Exit(2);
#endif
        }  
        
#endregion
    }
}
