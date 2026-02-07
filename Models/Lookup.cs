using System;
namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Lookup model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Lookup : BaseModel
	{
		/// <summary>
		/// Gets or sets the lookup identifier.
		/// </summary>
		/// <value>The lookup identifier.</value>
		public int LookupId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }

		/// <summary>
		/// Gets or sets the list order.
		/// </summary>
		/// <value>The list order.</value>
		public int ListOrder { get; set; }
	}
}
