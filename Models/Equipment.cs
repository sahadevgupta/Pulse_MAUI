using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse_MAUI.Models
{
    public class Equipment : BaseModel
    {

        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the unique serial number.
        /// </summary>
        /// <value>
        /// The unique serial number.
        /// </value>
        public string UniqueSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the last maintained.
        /// </summary>
        /// <value>
        /// The last maintained.
        /// </value>
        public DateTime? LastMaintained { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the active project.
        /// </summary>
        /// <value>
        /// The active project.
        /// </value>
        public int? ActiveProject { get; set; }

    }
}
