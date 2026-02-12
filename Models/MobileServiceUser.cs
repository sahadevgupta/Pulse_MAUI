using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Models
{
    public partial class MobileServiceUser : ObservableObject
    {
        [ObservableProperty]
        private string? _authenticationToken;

        [ObservableProperty]
        private string? _userId;

        [ObservableProperty]
        private string? _userName;
    }
}
