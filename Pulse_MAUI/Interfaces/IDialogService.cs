using Pulse_MAUI.Enums;

namespace Pulse_MAUI.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertDialog(string title, string message, AlertType alertType = AlertType.Warning, int timeout = 3500);
        void HideLoading();
        void ShowLoading(string message = "Loading");
        Task<bool> ShowConfirmAsync(string message, string title = "Pulse CMS", string accept = "Yes", string cancel = "No");
    }
}