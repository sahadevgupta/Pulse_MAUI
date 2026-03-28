using Pulse_MAUI.Services.Navigation;

namespace Pulse_MAUI.Interfaces
{
    public interface IViewModelParameters
    {
        IAppWorkflowManager AppWorkflowManager { get; }
        IConnectivityService ConnectivityService { get; }
        IDialogService DialogService { get; }
        INavigationService NavigationService { get; }
    }
}