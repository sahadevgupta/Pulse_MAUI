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
        public string AppName => Preferences.Get("AppTitle", "Pulse Mobile");
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
        }

        #region [ Methods && Service Calls ]

        private async Task InitializeDataAsync()
        {
            PopulateOptionsMenu();
            App.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                CurrentDate = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                return true;
            });
            await FetchDataCommand.ExecuteAsync(null);

        }

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

        private async Task PerformFullSyncAsync()
        {
            if (!AppHelpers.IsLoggedIn)
            {
                await DialogService.ShowAlertDialog("Login Error", "Invalid User Login", AlertType.Error);
                return;
            }

            Guid transactionBatchId = Guid.NewGuid();
            await _appWorkflowManager.SyncLogService.PostSyncLogStart(transactionBatchId);

            try
            {
                if (!await EnsureInternetAsync()) return;

                var blobConnectionString = await _appWorkflowManager.UserService.GetAzureBlobStorageString();

                // 1. Standing data sync (full or incremental)
                await SyncStandingDataAsync();

                // 2. Upload blobs
                if (!await EnsureInternetAsync()) return;
                DialogService.ShowLoading("Uploading Image Items");
                await _appWorkflowManager.SynchroniseService.UploadBlobData(blobConnectionString);

                // 3. Push/Pull data after images uploaded
                if (!await EnsureInternetAsync()) return;
                DialogService.ShowLoading(UserInterface.MenuPage_Synchronising);
                await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(true, true);

                // 4. Download blobs
                if (!await EnsureInternetAsync()) return;
                DialogService.ShowLoading("Downloading Image Items");
                await _appWorkflowManager.SynchroniseService.DownloadBlobData(blobConnectionString);

                // 5. Finish sync
                await _appWorkflowManager.SyncLogService.PostSyncLogFinish(transactionBatchId);
                var a = DateTime.UtcNow.ToString("dd-MM-yyyy, HH:mm:ss");

                AppHelpers.SyncDate = a;

                // Refresh user
                await _appWorkflowManager.UserService.FetchCurrentUser();
            }
            catch (DatasyncInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                await DialogService.ShowAlertDialog("Sync Error", ex.Message, AlertType.Error);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DialogService.ShowAlertDialog("Sync Error", "Unable to complete data sync", AlertType.Error);
            }
        }

        private async Task<bool> EnsureInternetAsync()
        {
            if (!ViewModelParameters.ConnectivityService.IsConnected)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return false;
            }
            return true;
        }

        private async Task SyncStandingDataAsync()
        {
            try
            {
                if (!await EnsureInternetAsync()) return;

                var projectData = await _appWorkflowManager.LookupService.GetProjectListAsync();

                bool isIncremental = projectData != null && projectData.Any();

                string loadingMsg = isIncremental
                    ? UserInterface.MenuPage_Synchronising
                    : UserInterface.MenuPage_Synchronising + " (Full)";

                DialogService.ShowLoading(loadingMsg);

                var syncResult = await _appWorkflowManager.SynchroniseService.PushAndPullDataAsync(isIncremental, false);

                if (syncResult?.Count > 0)
                {
                    var sb = new StringBuilder();
                    syncResult.ForEach(e => sb.AppendLine(e));
                    await DialogService.ShowAlertDialog("Sync Error", sb.ToString(), AlertType.Error);
                }
            }
            catch (DatasyncInvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await DialogService.ShowAlertDialog("Sync Error", "Unable to complete data sync", AlertType.Error);
            }
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task FetchData()
        {
            if (string.IsNullOrWhiteSpace(AppHelpers.AzureServiceUrl) || AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
                return;

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
            // if (selectedMenu.Route == nameof(ActivityListPage))
            // {
            //     await Shell.Current.GoToAsync("//activityroot/activitylist");

            // }
            // else
            await Shell.Current.GoToAsync($"//{selectedMenu.Route}");
            Shell.Current.FlyoutIsPresented = false;
        }

        [RelayCommand]
        private async Task SynchroniseData()
        {
            Shell.Current.FlyoutIsPresented = false;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DialogService.ShowAlertDialog("Alert!!", "No Internet Connection Available");
                return;
            }

            try
            {
                DialogService.ShowLoading("Authenticating User");
                await _appWorkflowManager.UserService.LoginAsync(AppHelpers.AzureServiceUrl);

                await PerformFullSyncAsync();

                DialogService.HideLoading();
                WeakReferenceMessenger.Default.Send(new NotificationMessageEvent(NotifyType.PostSyncRefresh));

                PopulateOptionsMenu();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region [ Override Methods ]

        public async override Task LoadDataOnNavigatedTo()
        {
            await InitializeDataAsync();
        }

        #endregion
    }
}
