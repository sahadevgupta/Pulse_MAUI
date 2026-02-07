using System;
namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Unit model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Unit : BaseModel
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
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the PUI.
		/// </summary>
		/// <value>The PUI.</value>
		public int PUId { get; set; }
	}
}
