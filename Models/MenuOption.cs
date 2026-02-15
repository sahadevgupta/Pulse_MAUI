using System;
namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Menu option model class.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class MenuOption
	{
		/// <summary>
		/// Gets or sets the icon source.
		/// </summary>
		/// <value>The icon source.</value>
		public string? IconSource { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string? Title { get; set; }

        public string? Route { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>The type of the target.</value>
        public Type? TargetType { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index { get; set; }
	}	
}
