using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private bool _isDirty;

        [ObservableProperty]
        private bool _isBusy;
    }
}
