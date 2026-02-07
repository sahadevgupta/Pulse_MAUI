using System;

using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
	public class ActivityAssigned : BaseModel
	{
		public string Name { get; set; }
		public int PCAId { get; set; }
		public int? PCCId { get; set; }
		public int? AssignedTo { get; set; }
		public DateTime? AssignedDate { get; set; }
		public string TagId { get; set; }
		public string ComponentType { get; set; }
	}
}
