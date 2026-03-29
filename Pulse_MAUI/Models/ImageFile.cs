using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Pulse_MAUI.Models
{
    /// <summary>
    /// Image File model class.
    /// Fogbugz Case:
    /// Author: Manuel Dambrine
    /// Created: 29/03/2013
    /// </summary>
    public partial class ImageFile : ObservableObject
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [ObservableProperty]
        private string? _url;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [ObservableProperty]
        private string? _description;

        /// <summary>
        /// Gets or sets the checklist step.
        /// </summary>
        /// <value>
        /// The checklist step.
        /// </value>
        public int? ChecklistStep { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [available to delete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [available to delete]; otherwise, <c>false</c>.
        /// </value>
        public bool AvailableToDelete { get; set; }

        /// <summary>
        /// Gets or sets the type of the control.
        /// </summary>
        /// <value>
        /// The type of the control.
        /// </value>
        public int? ControlType { get; set; }
    }
}
