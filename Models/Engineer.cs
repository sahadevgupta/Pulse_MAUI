using System;
using Microsoft.Datasync.Client;
using Newtonsoft.Json;
namespace Pulse_MAUI.Models
{
#nullable disable
	/// <summary>
	/// Engineer model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Engineer
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("deleted")]
		public bool Deleted { get; set; }

		[JsonProperty("updatedAt")]
		public DateTimeOffset UpdatedAt { get; set; }

		[JsonProperty("version")]
		public byte[] Version { get; set; }

		[JsonProperty("engineerId")]
		public int EngineerId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("emailAddress")]
		public string EmailAddress { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }
	}
}
