namespace Pulse_MAUI.Interfaces
{
    public interface IViewModelParameters
    {
        IAppWorkflowManager AppWorkflowManager { get; }
        IConnectivityService ConnectivityService { get; }
        IDialogService DialogService { get; }
        IShellNavigationService ShellNavigationService { get; }
    }
}