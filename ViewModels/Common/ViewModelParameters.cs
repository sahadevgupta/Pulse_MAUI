using Pulse_MAUI.Interfaces;

namespace Pulse_MAUI.ViewModels.Common
{
    public class ViewModelParameters : IViewModelParameters
    {
        public ViewModelParameters(IAppWorkflowManager appWorkflowManager,
            IConnectivityService connectivityService,
            IDialogService dialogService,
            IShellNavigationService shellNavigationService)
        {
            AppWorkflowManager = appWorkflowManager;
            ConnectivityService = connectivityService;
            DialogService = dialogService;
            ShellNavigationService = shellNavigationService;
        }

        public IAppWorkflowManager AppWorkflowManager { get; }
        public IConnectivityService ConnectivityService { get; }
        public IDialogService DialogService { get; }
        public IShellNavigationService ShellNavigationService { get; }
    }

}
