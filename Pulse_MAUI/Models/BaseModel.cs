using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Datasync.Client;
using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
    public abstract partial class BaseModel : ObservableObject, IDirty
    {
        public string? Id { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public byte[]? Version { get; set; }

        [JsonIgnore]
        public bool IsDirty
        {
            get;
            set;
        }
    }
}
