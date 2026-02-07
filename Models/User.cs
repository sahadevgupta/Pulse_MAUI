using System;
using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// User model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class User : BaseModel
	{
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the apex identifier.
		/// </summary>
		/// <value>The apex identifier.</value>
		public string ApexId { get; set; }

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

		/// <summary>
		/// Gets or sets the default time zone identifier.
		/// </summary>
		/// <value>The default time zone identifier.</value>
		public string DefaultTimeZoneId { get; set; }
	}
}
