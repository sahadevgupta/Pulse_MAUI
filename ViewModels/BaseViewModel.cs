using CommunityToolkit.Mvvm.ComponentModel;
using Pulse_MAUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class BaseViewModel(IDialogService dialogService) : ObservableObject
    {
        protected readonly IDialogService DialogService = dialogService;

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private bool _isDirty;

        [ObservableProperty]
        private bool _isBusy;
    }
}
