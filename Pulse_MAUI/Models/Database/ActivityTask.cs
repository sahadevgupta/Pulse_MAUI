namespace Pulse_MAUI.Models
{
    /// <summary>
    /// Activity Task model class.
    /// Fogbugz Case:
    /// Author: Manuel Dambrine
    /// Created: 29/03/2013
    /// </summary>
    public class ActivityTask : BaseModel
    {
        /// <summary>
        /// Gets or sets the check list identifier.
        /// </summary>
        /// <value>The check list identifier.</value>
        public int CheckListId { get; set; }

        /// <summary>
        /// Gets or sets the activity identifier.
        /// </summary>
        /// <value>The activity identifier.</value>
        public int? ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>The project identifier.</value>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the task description.
        /// </summary>
        /// <value>The task description.</value>
        public string TaskDescription { get; set; }

        /// <summary>
        /// Gets or sets the expected result.
        /// </summary>
        /// <value>The expected result.</value>
        public string ExpectedResult { get; set; }

        /// <summary>
        /// Gets or sets the actual result.
        /// </summary>
        /// <value>The actual result.</value>
        public string ActualResult { get; set; }


        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>The step.</value>
        public int? Step { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the engineer.
        /// </summary>
        /// <value>The engineer.</value>
        public int? Engineer { get; set; }

        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        /// <value>The equipment.</value>
        public int? Equipment { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int? Status { get; set; }

        /// <summary>
        /// Gets or sets the date recorded.
        /// </summary>
        /// <value>The date recorded.</value>
        public DateTime? DateRecorded { get; set; }

        /// <summary>
        /// Gets or sets the has log.
        /// </summary>
        /// <value>The has log.</value>
        public bool? HasLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Pulse_MAUI.Models.ActivityTask"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }


        /// <summary>
        /// Gets or sets the last latitude.
        /// </summary>
        /// <value>
        /// The last latitude.
        /// </value>
        public float? LastLatitude { get; set; }

        /// <summary>
        /// Gets or sets the last longitude.
        /// </summary>
        /// <value>
        /// The last longitude.
        /// </value>
        public float? LastLongitude { get; set; }

    }
}
