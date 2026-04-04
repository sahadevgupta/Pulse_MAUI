using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Services.Navigation;

namespace Pulse_MAUI.ViewModels
{
    public partial class BaseViewModel(IViewModelParameters viewModelParameters) : ObservableObject, IQueryAttributable
    {
        protected readonly IViewModelParameters ViewModelParameters = viewModelParameters;
        protected readonly INavigationService NavigationService = viewModelParameters.NavigationService;
        protected readonly IDialogService DialogService = viewModelParameters.DialogService;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private bool _isDirty;

        [ObservableProperty]
        private bool _isBusy;

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {

        }

        #region [ Methods ]

        public virtual Task LoadDataOnAppearing()
        {
            return Task.CompletedTask;
        }
        public virtual Task LoadDataOnDisappearing()
        {
            return Task.CompletedTask;
        }
        public virtual Task LoadDataOnNavigatedTo()
        {
            return Task.CompletedTask;
        }

        protected void HandleException(Exception exception)
        {
            SentrySdk.CaptureException(exception);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task Back()
        {
            await NavigationService.NavigateBack();
        }

        [RelayCommand]
        private void ShowFlyout()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        #endregion

    }
}
