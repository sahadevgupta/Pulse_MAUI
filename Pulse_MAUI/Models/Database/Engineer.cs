using System;
using Microsoft.Datasync.Client;
using Newtonsoft.Json;
using Pulse_MAUI.Models.Database;
namespace Pulse_MAUI.Models
{
#nullable disable
	/// <summary>
	/// Engineer model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Engineer : BaseSyncModel
	{
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
