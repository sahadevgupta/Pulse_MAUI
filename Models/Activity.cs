using System;

using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
    [Microsoft.Datasync.Client.DataTable("activity")]
	public class Activity
	{
        public bool deleted { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string version { get; set; }
        public string id { get; set; }
        public object lastLongitude { get; set; }
        public object lastLatitude { get; set; }
        public int phaseId { get; set; }
        public int leadDisciplineId { get; set; }
        public int pucId { get; set; }
        public int puId { get; set; }
        public object dateCompleted { get; set; }
        public DateTime? dateRecorded { get; set; }
        public string modifiedBy { get; set; }
        public string status { get; set; }
        public int statusId { get; set; }
        public string unit { get; set; }
        public string commissioningSystem { get; set; }
        public string componentType { get; set; }
        public string tagId { get; set; }
        public DateTime assignedEngineerDate { get; set; }
        public int assignedEngineerId { get; set; }
        public int pccId { get; set; }
        public int pcaId { get; set; }
        public string name { get; set; }
        public int projectId { get; set; }
    }

	/// <summary>
	/// Activity model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	//public class Activity : BaseModel
	//{

 //       /// <summary>
 //       /// Gets or sets the ProjectId.
 //       /// </summary>
 //       /// <value>The tag identifier.</value>
 //       public int? ProjectId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the name.
 //       /// </summary>
 //       /// <value>The name.</value>
 //       public string? Name { get; set; }

 //       /// <summary>
 //       /// Gets or sets the PCAId.
 //       /// </summary>
 //       /// <value>The PCAId.</value>
 //       public int PCAId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the PCCId.
 //       /// </summary>
 //       /// <value>The PCCId.</value>
 //       public int? PCCId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the assigned engineer identifier.
 //       /// </summary>
 //       /// <value>The assigned engineer identifier.</value>
 //       public int? AssignedEngineerId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the assigned engineer date.
 //       /// </summary>
 //       /// <value>The assigned engineer date.</value>
 //       public DateTime? AssignedEngineerDate { get; set; }

 //       /// <summary>
 //       /// Gets or sets the tag identifier.
 //       /// </summary>
 //       /// <value>The tag identifier.</value>
 //       public string? TagId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the type of the component.
 //       /// </summary>
 //       /// <value>The type of the component.</value>
 //       public string? ComponentType { get; set; }


 //       /// <summary>
 //       /// Gets or sets the commissioning system.
 //       /// </summary>
 //       /// <value>
 //       /// The commissioning system.
 //       /// </value>
 //       public string? CommissioningSystem { get; set; }


 //       /// <summary>
 //       /// Gets or sets the unit.
 //       /// </summary>
 //       /// <value>
 //       /// The unit.
 //       /// </value>
 //       public string? Unit { get; set; }

 //       /// <summary>
 //       /// Gets or sets the number of tasks for this activity
 //       /// </summary>
 //       /// <value>The tag identifier.</value>
 //       public int TaskCount { get; set; }

 //       /// <summary>
 //       /// Gets or sets the status identifier.
 //       /// </summary>
 //       /// <value>
 //       /// The status identifier.
 //       /// </value>
 //       public int StatusId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the status.
 //       /// </summary>
 //       /// <value>
 //       /// The status.
 //       /// </value>
 //       public string? Status { get; set; }


 //       /// <summary>
 //       /// Gets or sets the modfied by.
 //       /// </summary>
 //       /// <value>
 //       /// The modfied by.
 //       /// </value>
 //       public string? ModifiedBy { get; set; }

 //       /// <summary>
 //       /// Gets or sets the date recorded.
 //       /// </summary>
 //       /// <value>
 //       /// The date recorded.
 //       /// </value>
 //       public DateTime? DateRecorded { get; set; }

 //       /// <summary>
 //       /// Gets or sets the date completed.
 //       /// </summary>
 //       /// <value>
 //       /// The date completed.
 //       /// </value>
 //       public DateTime? DateCompleted { get; set; }

 //       /// <summary>
 //       /// Gets or sets the pu identifier.
 //       /// </summary>
 //       /// <value>
 //       /// The pu identifier.
 //       /// </value>
 //       public int PUId { get; set; }

 //       /// <summary>
 //       /// Gets or sets the puc identifier.
 //       /// </summary>
 //       /// <value>
 //       /// The puc identifier.
 //       /// </value>
 //       public int PUCId { get; set; }


 //       /// <summary>
 //       /// Gets or sets the lead discipline identifier.
 //       /// </summary>
 //       /// <value>
 //       /// The lead discipline identifier.
 //       /// </value>
 //       public int? LeadDisciplineId { get; set; }


 //       /// <summary>
 //       /// Gets or sets the last latitude.
 //       /// </summary>
 //       /// <value>
 //       /// The last latitude.
 //       /// </value>
 //       public float? LastLatitude { get; set; }

 //       /// <summary>
 //       /// Gets or sets the last longitude.
 //       /// </summary>
 //       /// <value>
 //       /// The last longitude.
 //       /// </value>
 //       public float? LastLongitude { get; set; }


 //   }
}
