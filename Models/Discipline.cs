using System;
using Newtonsoft.Json;

namespace Pulse_MAUI.Models
{
    public class Discipline : BaseModel
    {
        /// <summary>
        /// Gets or sets the punch identifier.
        /// </summary>
        /// <value>The punch identifier.</value>
              int? disciplineId;
              public int? DisciplineId
              {
                  get
                  {
                      return disciplineId;
                  }
                  set
                  {
                      SetPropertyChanged(ref disciplineId, value);
                  }
            }

        

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string description;
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

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        string status;
        public string Status
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

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string name;
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
    }
}
