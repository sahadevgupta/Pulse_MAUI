using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Services.Navigation;

namespace Pulse_MAUI.ViewModels.Common
{
    public class ViewModelParameters : IViewModelParameters
    {
        public ViewModelParameters(IAppWorkflowManager appWorkflowManager,
            IConnectivityService connectivityService,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            AppWorkflowManager = appWorkflowManager;
            ConnectivityService = connectivityService;
            DialogService = dialogService;
            NavigationService = navigationService;
        }

        public IAppWorkflowManager AppWorkflowManager { get; }
        public IConnectivityService ConnectivityService { get; }
        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }
    }

}
