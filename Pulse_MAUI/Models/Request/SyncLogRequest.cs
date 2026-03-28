using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Models.Request
{
    public class SyncLogRequest
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("deviceId")]
        public string? DeviceId { get; set; }

        [JsonProperty("platform")]
        public string? Platform { get; set; }

        [JsonProperty("model")]
        public string? Model { get; set; } 

        [JsonProperty("syncMode")]
        public string? SyncMode { get; set; }

        [JsonProperty("transactionBatchId")]
        public Guid TransactionBatchId { get; set; }

    }
}
