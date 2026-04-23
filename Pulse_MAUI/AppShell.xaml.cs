using System.Globalization;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Views;

namespace Pulse_MAUI
{
    public partial class AppShell : Shell
    {
        private bool _initialStartupItemApplied;
        private const double FlyoutHeaderBaseLeftPadding = 16;
        private const double FlyoutHeaderBaseTopPadding = 16;
        private const double FlyoutHeaderBaseRightPadding = 20;
        private const double FlyoutHeaderBaseBottomPadding = 10;
        private double _lastAppliedFlyoutTopInset = -1;
        private IDialogService? _dialogService => IPlatformApplication.Current?.Services.GetService<IDialogService>();
        public AppShell()
        {
            InitializeComponent();

            LoadMenuItems();

            Routing.RegisterRoute(nameof(ActivityPage), typeof(ActivityPage));
            //Routing.RegisterRoute(nameof(ActivityListPage), typeof(ActivityListPage));
            Routing.RegisterRoute(nameof(FileListPage), typeof(FileListPage));
            //Routing.RegisterRoute(nameof(ImportSettingsPage), typeof(ImportSettingsPage));
            //Routing.RegisterRoute(nameof(PunchListPage), typeof(PunchListPage));
            Routing.RegisterRoute(nameof(PunchPage), typeof(PunchPage));
        }

        private void LoadMenuItems()
        {

            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
            {
                AddFlyoutPage(nameof(ImportSettingsPage), "Import Settings", typeof(ImportSettingsPage));
            }
            else
            {
                AddFlyoutPage("activityroot" + "'\'" + "activitylist", "Activities", typeof(ActivityListPage));
                AddFlyoutPage("activityroot" + "'\'" + "punchlist", "Punch List", typeof(PunchListPage));

            }

        }

        private void AddFlyoutPage(string route, string title, Type pageType)
        {
            // Register route
            Routing.RegisterRoute(route, pageType);

            // Create FlyoutItem
            var item = new FlyoutItem()
            {
                Title = title,
                Route = route
            };

            // Add ShellContent
            item.Items.Add(new ShellContent
            {
                Route = route,
                ContentTemplate = new DataTemplate(pageType)
            });

            // Add to Shell
            Items.Add(item);
        }

        private void InitialiseAsync()
        {
            SetStartupItem();
            //ApplyNavigationPresentation(CurrentPage);
            //SyncShellChrome();
        }

        public void SetStartupItem()
        {
            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
                FlyoutIsPresented = false;
                CurrentItem = ImportSettingsItem;
                //ApplyNavigationPresentation(CurrentPage);
                //SyncShellChrome();
                return;
            }

            FlyoutBehavior = FlyoutBehavior.Flyout;
            FlyoutIsPresented = false;
            CurrentItem = ActivitiesItem;
        }

        internal void ApplyFlyoutTopInset(double topInsetDip)
        {
            var safeTopInset = Math.Max(0, topInsetDip);
            if (Math.Abs(_lastAppliedFlyoutTopInset - safeTopInset) < 0.5)
                return;

            _lastAppliedFlyoutTopInset = safeTopInset;

            var headerPadding = new Thickness(
                FlyoutHeaderBaseLeftPadding,
                FlyoutHeaderBaseTopPadding + safeTopInset,
                FlyoutHeaderBaseRightPadding,
                FlyoutHeaderBaseBottomPadding);


        }

        private static string FormatReminderSyncDate(string? storedValue)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
                return "Never";

            DateTimeOffset utcTimestamp;
            if (DateTimeOffset.TryParse(storedValue, out var parsedTimestamp))
            {
                utcTimestamp = parsedTimestamp.ToUniversalTime();
            }
            else if (DateTime.TryParseExact(
                         storedValue,
                         "dd-MMM-yyyy HH:mm:ss",
                         CultureInfo.InvariantCulture,
                         DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                         out var legacyTimestamp))
            {
                utcTimestamp = new DateTimeOffset(legacyTimestamp, TimeSpan.Zero);
            }
            else
            {
                return storedValue ?? "Never";
            }

            var localTimestamp = utcTimestamp.ToLocalTime();
            var timezoneCode = TimeZoneAbbreviationHelper.GetLocalTimeZoneCode(localTimestamp);
            return $"{localTimestamp:dd-MMM-yyyy HH:mm:ss} ({timezoneCode})";
        }

        #region  [ Override Methods ]

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
                return;

            if (AppHelpers.SyncDate.Length > 0)
            {
                Dispatcher.Dispatch(async () =>
                {
                    var reminderMessage = $"Remember to synchronise for the latest data.\n\nYour last sync was: {FormatReminderSyncDate(AppHelpers.SyncDate)}";

                    await DisplayAlertAsync("Pulse CMS", reminderMessage, "OK");

                });
            }
            else
            {
                Dispatcher.Dispatch(async () =>
                {
                    var reminderMessage = "Remember to synchronise for the latest data.\n\nYour last sync was: Never";
                    await DisplayAlertAsync("Pulse CMS", reminderMessage, "OK");
                });
            }
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            if (Handler is null)
                return;

            if (!_initialStartupItemApplied)
            {
                _initialStartupItemApplied = true;
                InitialiseAsync();
            }
        }
        #endregion

    }
}
