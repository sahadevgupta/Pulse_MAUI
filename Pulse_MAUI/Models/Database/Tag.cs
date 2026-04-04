using System;
using Pulse_MAUI.Models.Database;
namespace Pulse_MAUI.Models
{
	public class Tag : BaseSyncModel
	{
		public string? TagId { get; set; }
		public int? ExistingComponentId { get; set; }
		public int? CommissioningId { get; set; }
		public int? UnitId { get; set; }
		public int? TemplateComponentId { get; set; }
	}
}
