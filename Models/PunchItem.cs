using System;

using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// PunchItem model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class PunchItem : BaseModel
	{
		int? punchId;
        Guid? deviceReferenceID;

        const int PunchDescriptionMax = 100;

        /// <summary>
        /// Gets or sets the punch identifier.
        /// </summary>
        /// <value>The punch identifier.</value>
        [JsonProperty(PropertyName = "punchId")]
        public int? PunchId
		{
			get
			{
				return punchId;
			}
			set
			{
                    SetPropertyChanged(ref punchId, value);
			}
		}


        /// <summary>
        /// Gets the mobile identifier.
        /// </summary>
        /// <value>
        /// The mobile identifier.
        /// </value>
        public string MobileId
        {
            get
            {
                return Id;
            }
        }

		string description;

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		[JsonProperty(PropertyName = "description")]
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				SetPropertyChanged(ref description, value);
			}
		}

		int projectId;

		/// <summary>
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>The project identifier.</value>
		[JsonProperty(PropertyName = "projectId")]
		public int ProjectId
		{
			get
			{
				return projectId;
			}
			set
			{
				SetPropertyChanged(ref projectId, value);
			}
		}

		int? pUId;

		/// <summary>
		/// Gets or sets the PUId.
		/// </summary>
		/// <value>The PUId.</value>
		[JsonProperty(PropertyName = "pUId")]
		public int? PUId
		{
			get
			{
				return pUId;
			}
			set
			{
				SetPropertyChanged(ref pUId, value);
			}
		}

		int? pUCId;

		/// <summary>
		/// Gets or sets the PUCId.
		/// </summary>
		/// <value>The PUCId.</value>
		[JsonProperty(PropertyName = "pUCId")]
		public int? PUCId
		{
			get
			{
				return pUCId;
			}
			set
			{
				SetPropertyChanged(ref pUCId, value);
			}
		}

		string tagId;

		/// <summary>
		/// Gets or sets the tag identifier.
		/// </summary>
		/// <value>The tag identifier.</value>
		[JsonProperty(PropertyName = "tagId")]
		public string TagId
		{
			get
			{
				return tagId;
			}
			set
			{
				SetPropertyChanged(ref tagId, value);
			}
		}

		string componentType;

		/// <summary>
		/// Gets or sets the type of the component.
		/// </summary>
		/// <value>The type of the component.</value>
		[JsonProperty(PropertyName = "componentType")]
		public string ComponentType
		{
			get
			{
				return componentType;
			}
			set
			{
				SetPropertyChanged(ref componentType, value);
			}
		}

		string pIdReference;

		/// <summary>
		/// Gets or sets the PId reference.
		/// </summary>
		/// <value>The PId reference.</value>
		[JsonProperty(PropertyName = "pIdReference")]
		public string PIdReference
		{
			get
			{
				return pIdReference;
			}
			set
			{
				SetPropertyChanged(ref pIdReference, value);
			}
		}

		DateTime createdOn;

		/// <summary>
		/// Gets or sets the created on.
		/// </summary>
		/// <value>The created on.</value>
		[JsonProperty(PropertyName = "createdOn")]
		public DateTime CreatedOn
		{
			get
			{
				return createdOn;
			}
			set
			{
				SetPropertyChanged(ref createdOn, value);
			}
		}

		string createdBy;

		/// <summary>
		/// Gets or sets the created by.
		/// </summary>
		/// <value>The created by.</value>
		[JsonProperty(PropertyName = "createdBy")]
		public string CreatedBy
		{
			get
			{
				return createdBy;
			}
			set
			{
				SetPropertyChanged(ref createdBy, value);
			}
		}

		DateTime? updatedOn;

		/// <summary>
		/// Gets or sets the updated on.
		/// </summary>
		/// <value>The updated on.</value>
		[JsonProperty(PropertyName = "updatedOn")]
		public DateTime? UpdatedOn
		{
			get
			{
				return updatedOn;
			}
			set
			{
				SetPropertyChanged(ref updatedOn, value);
			}
		}

		string updatedBy;

		/// <summary>
		/// Gets or sets the updated by.
		/// </summary>
		/// <value>The updated by.</value>
		[JsonProperty(PropertyName = "updatedBy")]
		public string UpdatedBy
		{
			get
			{
				return updatedBy;
			}
			set
			{
				SetPropertyChanged(ref updatedBy, value);
			}
		}


		int status;

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[JsonProperty(PropertyName = "status")]
		public int Status
		{
			get
			{
				return status;
			}
			set
			{
				SetPropertyChanged(ref status, value);
			}
		}


        string statusString;
        [JsonIgnore] // doesnt get passed in the sync
        public string StatusString
        {

            get
            {
                return statusString;
            }

            set
            {
                value = statusString;
            }

        }


		string comments;
		/// <summary>
		/// Gets or sets the comments.
		/// </summary>
		/// <value>The comments.</value>
		[JsonProperty(PropertyName = "comments")]
		public string Comments
		{
			get
			{
				return comments;
			}
			set
			{
				SetPropertyChanged(ref comments, value);
			}
		}

		int? pCCId;

		/// <summary>
		/// Gets or sets the PCCId.
		/// </summary>
		/// <value>The PCCId.</value>
		[JsonProperty(PropertyName = "pCCId")]
		public int? PCCId
		{
			get
			{
				return pCCId;
			}
			set
			{
				SetPropertyChanged(ref pCCId, value);
			}
		}

		int? pCAId;

		/// <summary>
		/// Gets or sets the PCAId.
		/// </summary>
		/// <value>The PCAId.</value>
		[JsonProperty(PropertyName = "pCAId")]
		public int? PCAId
		{
			get
			{
				return pCAId;
			}
			set
			{
				SetPropertyChanged(ref pCAId, value);
			}
		}

		int? controltype;

		/// <summary>
		/// Gets or sets the controltype.
		/// </summary>
		/// <value>The controltype.</value>
		[JsonProperty(PropertyName = "controltype")]
		public int? Controltype
		{
			get
			{
				return controltype;
			}
			set
			{
				SetPropertyChanged(ref controltype, value);
			}
		}

		DateTime? targetCompleteBy;

		/// <summary>
		/// Gets or sets the target complete by.
		/// </summary>
		/// <value>The target complete by.</value>
		[JsonProperty(PropertyName = "targetCompleteBy")]
		public DateTime? TargetCompleteBy
		{
			get
			{
				return targetCompleteBy;
			}
			set
			{
				SetPropertyChanged(ref targetCompleteBy, value);
			}
		}

		int? priority;

		/// <summary>
		/// Gets or sets the priority.
		/// </summary>
		/// <value>The priority.</value>
		[JsonProperty(PropertyName = "priority")]
		public int? Priority
		{
			get
			{
				return priority;
			}
			set
			{
				SetPropertyChanged(ref priority, value);
			}
		}


        /// <summary>
        /// Gets or sets the assigned engineer identifier.
        /// </summary>
        /// <value>The assigned to  engineer identifier.</value>
        public int? AssignedToEngineer { get; set; }

		/// <summary>
		/// Gets or sets the assigned engineer date.
		/// </summary>
		/// <value>The assigned engineer date.</value>
		public DateTime? AssignedEngineerDate { get; set; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        [JsonIgnore] // doesnt get passed in the sync
        public string Title
        {
            get
            {

                if (description.Length > PunchDescriptionMax)
                {
                    return string.Format("{0}. {1}, {2}",
                                       ComponentType,
                                       TagId,
                                       Description.Substring(0, 100) + "...");
                }
                else
                {

                    return string.Format("{0}. {1}, {2}",
                                         ComponentType,
                                         TagId,
                                         Description);
                }
            }
        }

        /// <summary>
        /// Gets the sub title.
        /// </summary>
        /// <value>The sub title.</value>
        [JsonIgnore] // doesnt get passed in the sync
        public string SubTitle
		{
			get
			{
				return string.Format("{0}, {1}",
				                     CreatedOn.ToString("R"),
									 StatusString);
			}
		}

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [JsonIgnore] // doesnt get passed in the sync
        public string Unit { get; set; }


        /// <summary>
        /// Gets or sets the commissioning system.
        /// </summary>
        /// <value>
        /// The commissioning system.
        /// </value>
        /// 
        [JsonIgnore] // doesnt get passed in the sync
        public string CommissioningSystem { get; set; }


        /// <summary>
        /// Gets or sets the discipine identifier.
        /// </summary>
        /// <value>
        /// The discipine identifier.
        /// </value>
        public int? DisciplineId { get; set; }



    }
}
