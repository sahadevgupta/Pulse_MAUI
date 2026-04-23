using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pulse_MAUI.Models
{
    public partial class SyncStepModel : ObservableObject
    {

        [ObservableProperty] private string? _status = "[ ]";  // e.g., "OK", "[ ]", "[...]"
        [ObservableProperty] private string _groupName = string.Empty;
        [ObservableProperty] private bool _showGroupHeader;
        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _description = string.Empty;
        [ObservableProperty] private bool _isCurrent;
        [ObservableProperty] private bool _isCompleted;
    }
}
