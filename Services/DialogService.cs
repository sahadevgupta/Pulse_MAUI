using Mopups.Interfaces;
using Mopups.Services;
using Pulse_MAUI.Extensions;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Popups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class DialogService(ILoadingService lodingService, IPopupNavigation popupNavigation) : IDialogService
    {
        public async Task ShowAlertDialog(string message, bool response = true, int timeout = 3500)
        {
            var popup = new CustomDialogPopup
            {
                Message = message,
                Icon = response ? FontAwesomeIcons.CheckCircle : FontAwesomeIcons.TimesCircle,
                IconTintColor = response ? (Color)Application.Current!.Resources["Green"] : (Color)Application.Current!.Resources["Red"]
            };

            bool isMyPopupOpen = popupNavigation.PopupStack
                                                .Any(p => p is CustomDialogPopup);
            if (!isMyPopupOpen)
            {
                await popupNavigation.PushAsync(popup);
                System.Threading.Timer timer = null;
                timer = new System.Threading.Timer((obj) =>
                {
                    popupNavigation.PopAsync();
                    timer.Dispose();
                }, null, timeout, System.Threading.Timeout.Infinite);
            }
        }
        public void ShowLoading()
        {
            lodingService.ShowLoading();
        }

        public void HideLoading()
        {
            lodingService.HideLoading();
        }
    }
}
