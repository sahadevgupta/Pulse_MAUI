using System;

using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Base model containing azure specific fields and other additional fields.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public abstract class BaseModel : BaseNotify, IDirty
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the updated at.
		/// </summary>
		/// <value>The updated at.</value>
		public DateTimeOffset? UpdatedAt { get; set; }

		/// <summary>
		/// Gets or sets the created at.
		/// </summary>
		/// <value>The created at.</value>
		public DateTimeOffset? CreatedAt { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		public byte[] Version { get; set; }

        
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:Pulse_MAUI.Models.BaseModel"/> is dirty.
		/// </summary>
		/// <value><c>true</c> if is dirty; otherwise, <c>false</c>.</value>
		[JsonIgnore]
		public bool IsDirty
		{
			get;
			set;
		}
	}
}
