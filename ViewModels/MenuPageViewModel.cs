using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Datasync.Client;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Events;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class MenuPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        readonly IAppWorkflowManager _appWorkflowManager;
        readonly IActivityService _activityService;
        readonly IActivitySearchService _activitySearchService;
        readonly IPunchSearchService _punchSearchService;
        //readonly ILookupService _lookupService;
        //readonly IUserService _userService;
        //readonly ISyncLogService _syncLogService;
        //readonly ISynchroniseService _synchroniseService;

        [ObservableProperty]
        ObservableCollection<MenuOption>? _optionsItems;

        [ObservableProperty]
        private string? _currentDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");

        public string ProfileName
        {
            get
            {
                return string.Format(UserInterface.MenuPage_HelloTitle,
                                     _appWorkflowManager.EngineerService.CurrentEngineer != null ?
                                     _appWorkflowManager.EngineerService.CurrentEngineer.Name :
                                     "N/A");
            }

        }
        public string AppName =>Preferences.Get("AppTitle", "Pulse Mobile");
        public string AppVersion => string.Format("Version {0}", VersionTracking.CurrentVersion);

        #endregion
        public MenuPageViewModel(IViewModelParameters viewModelParameters,
            IActivityService activityService,
            IActivitySearchService activitySearchService,
            IPunchSearchService punchSearchService) : base(viewModelParameters)
        {
            _appWorkflowManager = viewModelParameters.AppWorkflowManager;
            _activityService = activityService;
            _activitySearchService = activitySearchService;
            _punchSearchService = punchSearchService;

            PopulateOptionsMenu();

            App.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.OnPropertyChanged("CurrentDate");
                return true;
            });

            FetchDataCommand.Execute(null);

        }

        #region [ Methods && Service Calls ]

        /// <summary>
		/// Populate the items for the options menu.
		/// </summary>
		private void PopulateOptionsMenu()
        {
            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
            {
                OptionsItems = new ObservableCollection<MenuOption>
                {
                    new MenuOption{
                    Title = "Import Settings",
                    Route = nameof(ImportSettingsPage),
                    TargetType = typeof(ImportSettingsPage),
                    Index = 2
                    }
                };
            }
            else
            {
                OptionsItems = new ObservableCollection<MenuOption>
                {
                  new MenuOption{
                      Title = "Activities",
                      Route = nameof(ActivityListPage),
                      TargetType = typeof(ActivityListPage),
                      Index = 0
                  }
                  ,new MenuOption{
                      Title = "Punch List",
                      Route = nameof(PunchListPage),
                      TargetType = typeof(PunchListPage),
                      Index = 1
                  }
                };
            }
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task FetchData()
        {
            if (_appWorkflowManager.EngineerService.CurrentEngineer == null)
                await _appWorkflowManager.EngineerService.FetchCurrentEngineer();

            if (_appWorkflowManager.UserService.CurrentUser == null)
                await _appWorkflowManager.UserService.FetchCurrentUser();

            
            this.OnPropertyChanged(nameof(ProfileName));
            this.OnPropertyChanged(nameof(CurrentDate));
        }

        [RelayCommand]
        private async Task MenuSelected(MenuOption selectedMenu)
        {
            await Shell.Current.GoToAsync($"//{selectedMenu.Route}");
            Shell.Current.FlyoutIsPresented = false;
        }

        [RelayCommand]
        private async Task SynchroniseData()
        {
            Shell.Current.FlyoutIsPresented = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await ViewModelParameters.DialogService.ShowAlertDialog("Alert!!","No Internet Connection Available");
                return;
            }

            ViewModelParameters.DialogService.ShowLoading("Authenticating User");
            await _appWorkflowManager.UserService.LoginAsync(AppHelpers.AzureServiceUrl);

            if (AppHelpers.IsLoggedIn)
            {
                // Create and display the value of two GUIDs.
                Guid TransactionBatchId = Guid.NewGuid();

                await _appWorkflowManager.SyncLogService.PostSyncLogStart(TransactionBatchId);

                var blobConnectionString = await _appWorkflowManager.UserService.GetAzureBlobStorageString();

                if (ViewModelParameters.ConnectivityService.IsConnected)
                {

                    List<string> result = new List<string>();
                    // Service failure handling here!
                    try
                    {
                        //Do a full or incremental pull of the standing data
                        IEnumerable<Project> projectData = await _appWorkflowManager.LookupService.GetProjectListAsync();

                        if (projectData != null && projectData.Count() > 0)
                        {
                            ViewModelParameters.DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
                            result = await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, false);
                        }
                        else
                        {
                            ViewModelParameters.DialogService.ShowLoading(UserInterface.MenuPage_Synchronising + " (Full)");
                            result = await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(false, false);
                        }

                    }
                    catch (DatasyncInvalidOperationException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        await ViewModelParameters.DialogService.ShowAlertDialog("Sync Error","Unable to complete data sync",AlertType.Error );
                    }


                    if (result.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (String err in result)
                        {
                            sb.AppendLine(err);
                        }

                        await ViewModelParameters.DialogService.ShowAlertDialog("Sync Error", sb.ToString(),AlertType.Error);
                    }


                }
                else
                {
                    await ViewModelParameters.DialogService.ShowAlertDialog("Alert!!","No Internet Connection Available");
                    return;
                }


                if (ViewModelParameters.ConnectivityService.IsConnected)
                {
                    ViewModelParameters.DialogService.ShowLoading("Uploading Image Items");
                    await _appWorkflowManager.SynchroniseService.UploadBlobData(blobConnectionString);
                }
                else
                {
                    await ViewModelParameters.DialogService.ShowAlertDialog("Alert!!","No Internet Connection Available");
                    return;
                }

                if (ViewModelParameters.ConnectivityService.IsConnected)
                {
                    ViewModelParameters.DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
                    await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, true);
                }
                else
                {
                    await ViewModelParameters.DialogService.ShowAlertDialog("Alert!!","No Internet Connection Available");
                    return;
                }


                if (ViewModelParameters.ConnectivityService.IsConnected)
                {
                    ViewModelParameters.DialogService.ShowLoading("Downloading Image Items");
                    await _appWorkflowManager.SynchroniseService.DownloadBlobData(blobConnectionString);
                }
                else
                {
                    await ViewModelParameters.DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                    return;
                }

                await _appWorkflowManager.SyncLogService.PostSyncLogFinish(TransactionBatchId);
                // store the last sync date
                AppHelpers.SyncDate = DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss");


                await _appWorkflowManager.UserService.FetchCurrentUser();
                var currentUser = _appWorkflowManager.UserService.CurrentUser;

                //if (currentUser != null)
                //{

                //    try
                //    {

                //        await Task.Delay(2000);
                //        // implement the required lists
                //        await _activityService.FetchActivityListAsync();
                //        await _appWorkflowManager.PunchService.FetchPunchListAsync();
                //        await _appWorkflowManager.UserService.FetchCurrentUser();
                //        await _appWorkflowManager.EngineerService.FetchCurrentEngineer();
                //        await Task.Delay(2000);

                //        await _activitySearchService.FetchSearchItems(_activityService.Activities);
                //        await _punchSearchService.FetchSearchItems();

                //        this.OnPropertyChanged("ProfileName");
                //        this.OnPropertyChanged("CurrentDate");
                //    }

                //    catch (Exception ex)
                //    {
                //        await ViewModelParameters.DialogService.ShowAlertDialog("Sync Error",ex.Message,AlertType.Error);
                //    }
                //}
                //else
                //{
                //    // Display warning and force user logout
                //    await ViewModelParameters.DialogService.ShowAlertDialog("Warning","You are not a registered user" );
                //    await _appWorkflowManager.UserService.LogoutAsync();
                //}

                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    //if (ViewModelParameters.ConnectivityService.IsConnected)
                    // {
                    //   await UserService.Instance.LogoutAsync();
                    // }
                }



            }
            else
            {
                // invalid user
                await ViewModelParameters.DialogService.ShowAlertDialog("Login Error", "Invalid User Login", AlertType.Error);
            }

            ViewModelParameters.DialogService.HideLoading();
            WeakReferenceMessenger.Default.Send(new NotificationMessageEvent( NotifyType.PostSyncRefresh));
        }
       
        #endregion
    }
}
