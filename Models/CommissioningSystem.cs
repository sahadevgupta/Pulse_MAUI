using System;
namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Commissioning system model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CommissioningSystem : BaseModel
	{
		/// <summary>
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>The project identifier.</value>
		public int? ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the unit identifier.
		/// </summary>
		/// <value>The unit identifier.</value>
		public int? UnitId { get; set; }

		/// <summary>
		/// Gets or sets the commissioning identifier.
		/// </summary>
		/// <value>The commissioning identifier.</value>
		public int? CommissioningId { get; set; }

		/// <summary>
		/// Gets or sets the PUCId.
		/// </summary>
		/// <value>The PUCId.</value>
		public int PUCId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>The start date.</value>
		public DateTime? StartDate { get; set; }

		/// <summary>
		/// Gets or sets the end date.
		/// </summary>
		/// <value>The end date.</value>
		public DateTime? EndDate { get; set; }

		/// <summary>
		/// Gets or sets the PUI.
		/// </summary>
		/// <value>The PUI.</value>
		public int? PUId { get; set; }

		/// <summary>
		/// Gets or sets the cloned from PUCI.
		/// </summary>
		/// <value>The cloned from PUCI.</value>
		public int? ClonedFromPUCId { get; set; }
        

    }
}
