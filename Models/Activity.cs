using System;

using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Activity model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Activity : BaseModel
	{

        /// <summary>
        /// Gets or sets the ProjectId.
        /// </summary>
        /// <value>The tag identifier.</value>
        [JsonProperty("projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PCAId.
        /// </summary>
        /// <value>The PCAId.</value>
        [JsonProperty("pcaId")]
        public int PCAId { get; set; }

        /// <summary>
        /// Gets or sets the PCCId.
        /// </summary>
        /// <value>The PCCId.</value>
        [JsonProperty("pccId")]
        public int? PCCId { get; set; }

        /// <summary>
        /// Gets or sets the assigned engineer identifier.
        /// </summary>
        /// <value>The assigned engineer identifier.</value>
        [JsonProperty("assignedEngineerId")]
        public int? AssignedEngineerId { get; set; }

        /// <summary>
        /// Gets or sets the assigned engineer date.
        /// </summary>
        /// <value>The assigned engineer date.</value>
        [JsonProperty("assignedEngineerDate")]
        public DateTime? AssignedEngineerDate { get; set; }

        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>The tag identifier.</value>
        [JsonProperty("tagId")]
        public string? TagId { get; set; }

        /// <summary>
        /// Gets or sets the type of the component.
        /// </summary>
        /// <value>The type of the component.</value>
        [JsonProperty("componentType")]
        public string? ComponentType { get; set; }




        /// <summary>
        /// Gets or sets the commissioning system.
        /// </summary>
        /// <value>
        /// The commissioning system.
        /// </value>
        [JsonProperty("commissioningSystem")]
        public string? CommissioningSystem { get; set; }


        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [JsonProperty("unit")]
        public string? Unit { get; set; }

        /// <summary>
        /// Gets or sets the number of tasks for this activity
        /// </summary>
        /// <value>The tag identifier.</value>
        public int TaskCount { get; set; }

        /// <summary>
        /// Gets or sets the status identifier.
        /// </summary>
        /// <value>
        /// The status identifier.
        /// </value>
        [JsonProperty("statusId")]
        public int StatusId { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("status")]
        public string? Status { get; set; }


        /// <summary>
        /// Gets or sets the modfied by.
        /// </summary>
        /// <value>
        /// The modfied by.
        /// </value>
        [JsonProperty("modifiedBy")]
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the date recorded.
        /// </summary>
        /// <value>
        /// The date recorded.
        /// </value>
        [JsonProperty("dateRecorded")]
        public DateTime? DateRecorded { get; set; }

        /// <summary>
        /// Gets or sets the date completed.
        /// </summary>
        /// <value>
        /// The date completed.
        /// </value>
        [JsonProperty("dateCompleted")]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the pu identifier.
        /// </summary>
        /// <value>
        /// The pu identifier.
        /// </value>
        [JsonProperty("puId")]
        public int PUId { get; set; }

        /// <summary>
        /// Gets or sets the puc identifier.
        /// </summary>
        /// <value>
        /// The puc identifier.
        /// </value>
        [JsonProperty("pucId")]
        public int PUCId { get; set; }


        /// <summary>
        /// Gets or sets the lead discipline identifier.
        /// </summary>
        /// <value>
        /// The lead discipline identifier.
        /// </value>
        [JsonProperty("leadDisciplineId")]
        public int? LeadDisciplineId { get; set; }


        /// <summary>
        /// Gets or sets the last latitude.
        /// </summary>
        /// <value>
        /// The last latitude.
        /// </value>
        [JsonProperty("lastLatitude")]
        public float? LastLatitude { get; set; }

        /// <summary>
        /// Gets or sets the last longitude.
        /// </summary>
        /// <value>
        /// The last longitude.
        /// </value>
        [JsonProperty("lastLongitude")]
        public float? LastLongitude { get; set; }


    }
}
