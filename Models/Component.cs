using System;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Component model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class Component : BaseModel
	{
		/// <summary>
		/// Gets or sets the project identifier.
		/// </summary>
		/// <value>The project identifier.</value>
		public int? ProjectId { get; set; }

		/// <summary>
		/// Gets or sets the commissioning identifier.
		/// </summary>
		/// <value>The commissioning identifier.</value>
		public int? CommissioningId { get; set; }

		/// <summary>
		/// Gets or sets the component identifier.
		/// </summary>
		/// <value>The component identifier.</value>
		public int? ComponentId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the tag identifier.
		/// </summary>
		/// <value>The tag identifier.</value>
		public string TagId { get; set; }

		/// <summary>
		/// Gets or sets the PCCId.
		/// </summary>
		/// <value>The PCCId.</value>
		public int PCCId { get; set; }

		/// <summary>
		/// Gets or sets the PUCId.
		/// </summary>
		/// <value>The PUCId.</value>
		public int? PUCId { get; set; }

        /// <summary>
        /// Gets or sets the PNID identifier reference.
        /// </summary>
        /// <value>
        /// The p identifier reference.
        /// </value>
        public string PIdReference { get; set; }

    }
}
