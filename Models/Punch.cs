using Pulse_MAUI.Abstractions;

namespace Pulse_MAUI.Models
{
	public class Punch : TableData
	{
		public int ProjectId { get; set; }
		public int PunchId { get; set; }
		public int Unit { get; set; }
		public int CommissioningSystem { get; set; }
		public string Component { get; set; }
		public string ComponentTag { get; set; }
		public string PIdReference { get; set; }
		public int ActivityName { get; set; }
		public string Description { get; set; }
		public string Comments { get; set; }
		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public int Files { get; set; }
		public int Priority { get; set; }
		public int Status { get; set; }

		public string Title
		{
			get
			{
				return string.Format("{0}. {1}, {2}, {3}", 
				                     Priority.ToString(), 
				                     Component, 
				                     ComponentTag, 
				                     Comments);
			}
		}

		public string SubTitle
		{
			get
			{
				return string.Format("{0}, {1}",
				                     CreatedOn.ToString("R"),
									 "OPEN");
			}
		}
	}
}
