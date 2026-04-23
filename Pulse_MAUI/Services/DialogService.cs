using Mopups.Interfaces;
using Pulse_MAUI.Enums;
using Pulse_MAUI.Extensions;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Popups;

namespace Pulse_MAUI.Services
{

    public class DialogService(ILoadingService lodingService, IPopupNavigation popupNavigation) : IDialogService
    {
        public async Task ShowAlertDialog(string title, string message, AlertType alertType = AlertType.Warning, int timeout = 3500)
        {
            var popup = new CustomDialogPopup
            {
                AlertTitle = title,
                Message = message,
                Icon = alertType == AlertType.Warning ?
                       FontAwesomeIcons.ExclamationTriangle :
                      (alertType == AlertType.Success ? FontAwesomeIcons.CheckCircle : FontAwesomeIcons.TimesCircle),

                IconTintColor = alertType == AlertType.Warning ?
                       (Color)Application.Current!.Resources["DarkBlue"] :
                      (alertType == AlertType.Success ? (Color)Application.Current!.Resources["Green"] : Colors.Red),
            };

            bool isMyPopupOpen = popupNavigation.PopupStack
                                                .Any(p => p is CustomDialogPopup);
            if (!isMyPopupOpen)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await popupNavigation.PushAsync(popup);
                    System.Threading.Timer? timer = null;
                    timer = new System.Threading.Timer((obj) =>
                    {
                        // Check if the popup stack is not empty before popping
                        if (popupNavigation.PopupStack.Count > 0)
                        {
                            popupNavigation.PopAsync();
                        }
                        timer?.Dispose();
                    }, null, timeout, System.Threading.Timeout.Infinite);
                });
            }
        }

        public Task<bool> ShowConfirmAsync(string message, string title = "Pulse CMS",
            string accept = "Yes", string cancel = "No")
                => MainThread.InvokeOnMainThreadAsync(() => ShowConfirmCoreAsync(title, message, accept, cancel));
        public void ShowLoading(string message = "Loading...")
        {
            HideLoading();
            lodingService.ShowLoading(message);
        }

        public void HideLoading()
        {
            lodingService.HideLoading();
        }

        private static Task<bool> ShowConfirmCoreAsync(string title, string message, string accept, string cancel)
            => Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
    }
}
