using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Models
{
    public class PendingOperation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Table { get; set; }
        public string? ItemId { get; set; }
        public string? OperationType { get; set; } // insert / update / delete
        public string? PayloadJson { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
