using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Pulse_MAUI.Models.Database;

namespace Pulse_MAUI.Models
{
    public partial class Item : BaseSyncModel
    {
        [ObservableProperty]
        private string? _mimeType;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private int? _controlType;

        [ObservableProperty]
        private int? _recordId;

        [ObservableProperty]
        private int? _size;

        [ObservableProperty]
        private DateTime? _uploadTime;

        [ObservableProperty]
        private string? _uploadedBy;

        [ObservableProperty]
        private DateTime? _lastUpdateTime;

        [ObservableProperty]
        private int? _checkListStep;

        [ObservableProperty]
        private string? _lastUpdatedBy;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private int? _projectId;

        [ObservableProperty]
        private string? _azurePath;

        [ObservableProperty]
        private string? _localPath;

        [ObservableProperty]
        private string? _localReferenceID;
    }
}
