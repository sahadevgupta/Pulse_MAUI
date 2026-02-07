using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Models
{
    /// <summary>
    /// Items model class.
    /// Fogbugz Case:
    /// Author: Neil Backhurst
    /// Created: 29/03/2013
    /// </summary>
    public class Item : BaseModel
    {

       
        string name;
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The punch identifier.</value>
        [JsonProperty(PropertyName = "name")]

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                SetPropertyChanged(ref name, value);
            }
        }


        int? controlType;
        /// <summary>
        /// Gets or sets the Control Type.
        /// </summary>
        /// <value>The control type.</value>
        [JsonProperty(PropertyName = "controlType")]
        public int? ControlType {
            get
            {
                return controlType;
            }
            set
            {
                SetPropertyChanged(ref controlType, value);
            }
        }

        int? recordId;
        /// <summary>
        /// Gets or sets the RecordId.
        /// </summary>
        /// <value>The reference record id (PCAId,PunchId etc).</value>
        [JsonProperty(PropertyName = "recordId")]
        public int? RecordID {
            get
            {
                return recordId;
            }
            set
            {
                SetPropertyChanged(ref recordId, value);
            }
        }


        string mimeType;
        /// <summary>
        /// Gets or sets the Mime Type
        /// </summary>
        /// <value>The Mime Type of the Item.</value>
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType {
            get
            {
                return mimeType;
            }
            set
            {
                SetPropertyChanged(ref mimeType, value);
            }
                
        }

        int? size;
        /// <summary>
        /// Gets or sets the Size
        /// </summary>
        /// <value>The size of the Item.</value>
        [JsonProperty(PropertyName = "size")]
        public int? Size {
            get
            {
                return size;
            }
            set
            {
                SetPropertyChanged(ref size, value);
            }
        }

        DateTime? uploadTime;
        /// <summary>
        /// Gets or sets the Mime Type
        /// </summary>
        /// <value>The Mime Type of the Item.</value>
        [JsonProperty(PropertyName = "uploadTime")]
        public DateTime? UploadTime {
            get
            {
                return uploadTime;
            }
            set
            {
                SetPropertyChanged(ref uploadTime, value);
            }
        }


        string uploadedBy;
        /// <summary>
        /// Gets or sets the UploadedBy Date
        /// </summary>
        /// <value>The Uploaded Date.</value>
        [JsonProperty(PropertyName = "uploadedBy")]
        public string UploadedBy {
            get
            {
                return uploadedBy;
            }
            set
            {
                SetPropertyChanged(ref uploadedBy, value);
            }
        }


        DateTime? lastUpdateTime;
        /// <summary>
        /// Gets or sets the Last Update Time
        /// </summary>
        /// <value>The last update time.</value>
        [JsonProperty(PropertyName = "lastUpdateTime")]
        public DateTime? LastUpdateTime {
            get
            {
                return lastUpdateTime;
            }

            set
            {
                SetPropertyChanged(ref lastUpdateTime, value);
            }
        }

        int? checkListStep;
        /// <summary>
        /// Gets or sets the Checklist step
        /// </summary>
        /// <value>The Checklist step.</value>
        [JsonProperty(PropertyName = "checkListStep")]
        public int? CheckListStep {
            get
            {
                return checkListStep;
            }
            set
            {
                SetPropertyChanged(ref checkListStep, value);
            }
        }

        string description;
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        /// <value>The descriptionp.</value>
        [JsonProperty(PropertyName = "description")]
        public string Description {
            get
            {
                return description;
            }
            set
            {
                SetPropertyChanged(ref description, value);
            }
        }

        string lastUpdatedBy;
        /// <summary>
        /// Gets or sets the last updated by
        /// </summary>
        /// <value>The user who last updated the item.</value>
        [JsonProperty(PropertyName = "lastUpdatedBy")]
        public string LastUpdatedBy {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                SetPropertyChanged(ref lastUpdatedBy, value);
            }
        }

        int? projectId;
        /// <summary>
        /// Gets or sets the projectId
        /// </summary>
        /// <value>The project reference Id.</value>
        [JsonProperty(PropertyName = "projectId")]

        public int? ProjectId {
            get
            {
                return projectId;
            }
            set
            {
                SetPropertyChanged(ref projectId, value);
            }
        }

        string azurePath;
        /// <summary>
        /// Gets or sets the Blob Storage Path
        /// </summary>
        /// <value>The Blob storage path.</value>
        [JsonProperty(PropertyName = "azurePath")]
        public string AzurePath {
            get
            {
                return azurePath;
            }
            set
            {
                SetPropertyChanged(ref azurePath, value);
            }
        }


        /// <summary>
        /// Gets a flag to indicate a new item requiring Blob upload at sync
        /// </summary>
        /// <value>Is this a new blob item.</value>
        bool isNewBlob;
        [JsonIgnore] // doesnt get passed in the sync
        public bool IsNewBlob
        {
            get
            {
                return isNewBlob;
            }
            set
            {
                SetPropertyChanged(ref isNewBlob, value);
            }
        }




        /// <summary>
        /// Holds the localPath of any images added
        /// </summary>
        /// <value>Is this a new blob item.</value>
        string localPath;
        public string LocalPath
        {
            get
            {
                return localPath;
            }
            set
            {
                SetPropertyChanged(ref localPath, value);
            }
        }


        /// <summary>
        /// Gets or sets the local reference identifier.
        /// </summary>
        /// <value>
        /// The local reference identifier.
        /// </value>
        string localReferenceID;
        public string LocalReferenceID
        {
            get
            {
                return localReferenceID;
            }

            set
            {
                SetPropertyChanged(ref localReferenceID, value);
            }
        }

    }
}
