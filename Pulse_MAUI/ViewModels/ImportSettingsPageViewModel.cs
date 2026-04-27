using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Events;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.ViewModels
{
    public partial class ImportSettingsPageViewModel : BaseViewModel
    {
        #region [ Properties ]


        readonly IDataManager _dataManager;

        [ObservableProperty]
        private string? _serviceUrl;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        #endregion

        public ImportSettingsPageViewModel(IDataManager dataManager,
            IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {
            _dataManager = dataManager;

#if DEBUG
            _serviceUrl = "https://pulseargwebappmobile.azurewebsites.net";
#endif
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

        private static void RefreshMenuState()
        {
            if (Shell.Current is AppShell s) s.SetStartupItem();
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task ImportSettings()
        {
            StatusMessage = "Connecting…";
            DialogService.ShowLoading("Importing Service Settings..");

            try
            {
                if (ServiceUrl?.Length > 0 || ServiceUrl != @"http://")
                {
                    string customUrl = string.Empty;
                    var lastCharacter = ServiceUrl.Last();
                    if (ServiceUrl.EndsWith("/"))
                    {
                        customUrl = ServiceUrl + @"Mobile/ServiceSetting.xml";
                    }
                    else
                    {
                        customUrl = ServiceUrl + @"/Mobile/ServiceSetting.xml";
                    }
                    StatusMessage = "Signing in with Microsoft…";
                    var user = await _dataManager.LoginAsync(ServiceUrl);

                    if (user is object)
                    {
                        StatusMessage = "Retrieving service settings…";
                        var setting = await _dataManager.GetSettings();
                        ServiceInfo info = ReadSettingData(setting);

                        if (info.ServiceError.Length == 0)
                        {
                            AppHelpers.AppTitle = info.ServiceTitle;
                            AppHelpers.AzureServiceUrl = info.ServiceURL;
                            AppHelpers.BlobStorageName = info.StorageName;

                            StatusMessage = "Found Service: " + info.ServiceTitle;
                            // Please restart the application
                            DialogService.HideLoading();
                            RefreshMenuState();
                            if (Shell.Current != null)
                            {
                                await Shell.Current.GoToAsync("//activities");
                                await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.FlyoutIsPresented = false);
                            }

                            //var menuvm = ServiceHelper.GetService<MenuPageViewModel>();
                            //if (menuvm != null)
                            {
                                WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.StartSync));

                            }

                        }
                        else
                        {
                            StatusMessage = "No Service Found";
                        }
                    }
                    else
                    {
                        StatusMessage = "Unable to Authenticate User";
                    }

                }
                else
                {
                    StatusMessage = "Please enter the provided address";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Error: " + ex.Message;
            }
            finally
            {
                DialogService.HideLoading();
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

        #region [ Override Methods ]

        public override void LoadDataOnAppearing()
        {
            StatusMessage = string.Empty;
            base.LoadDataOnAppearing();
        }

        #endregion
    }
}
