using System;
namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Engineer model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Engineer : BaseModel
	{
		/// <summary>
		/// Gets or sets the engineer identifier.
		/// </summary>
		/// <value>The engineer identifier.</value>
		public int EngineerId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		/// <value>The email address.</value>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public string Status { get; set; }
	}
}
