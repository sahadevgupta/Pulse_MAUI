namespace Pulse_MAUI.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertDialog(string message, bool response = true, int timeout = 3500);
        void HideLoading();
        void ShowLoading();
    }
}