using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Resources.Languages;
using Pulse_MAUI.Views;

namespace Pulse_MAUI
{
    public partial class AppShell : Shell
    {
        private IDialogService? _dialogService => IPlatformApplication.Current?.Services.GetService<IDialogService>();
        public AppShell()
        {
            InitializeComponent();

            LoadMenuItems();

            //Routing.RegisterRoute(nameof(activity), typeof(ImportSettingsPage));
            Routing.RegisterRoute(nameof(ImportSettingsPage),typeof(ImportSettingsPage));
            Routing.RegisterRoute(nameof(PunchListPage),typeof(PunchListPage));
        }

        private void LoadMenuItems()
        {

            if (AppHelpers.AzureServiceUrl == "https://www.syncservice.com")
            {
                AddFlyoutPage(nameof(ImportSettingsPage), "Import Settings", typeof(ImportSettingsPage));
            }
            else 
            {
                AddFlyoutPage(nameof(ActivityListPage), "Activities", typeof(ActivityListPage));
                AddFlyoutPage(nameof(PunchListPage), "Punch List", typeof(PunchListPage));

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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (AppHelpers.SyncDate.Length > 0)
            {
                Dispatcher.Dispatch(async () =>
                {
                   await DisplayAlertAsync(UserInterface.Synchronise_Reminder, string.Format(UserInterface.Synchronise_Date, AppHelpers.SyncDate), "Ok");

                });
            }
            else
            {
                Dispatcher.Dispatch(async () =>
                {
                   await DisplayAlertAsync(UserInterface.Synchronise_Reminder,UserInterface.Synchronise_DateNone,"Ok");
                });
            }
        }
    }
}
