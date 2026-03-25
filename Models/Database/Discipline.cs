using System;
using Newtonsoft.Json;
using Pulse_MAUI.Models.Database;

namespace Pulse_MAUI.Models
{
    public class Discipline : BaseSyncModel
    {
        public int? DisciplineId { get; set; }
        public string? Description { get; set; }

        public string? Status { get; set; }
        public string? Name { get; set; }

    }
}

