using CommunityToolkit.Mvvm.ComponentModel;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class BaseViewModel(IViewModelParameters viewModelParameters) : ObservableObject
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

        #endregion
    }
}
