using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    /// <summary>
	/// Class for generic engineer functionality
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class EngineerService(IDataManager dataManager)
    {
        private static EngineerService instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EngineerService Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = new EngineerService();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the current engineer.
        /// </summary>
        /// <value>The current engineer.</value>
        public Engineer CurrentEngineer { get; set; }


        /// <summary>
        /// Fetchs the current logged in engineer details and populates the CurrentEngineer field.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task FetchCurrentEngineer()
        {
            //var engineers = await dataManager.GetAllEngindeersAsync();

            //if (engineers != null)
            //{
            //    var availableEngineers = engineers.ToList();

            //    CurrentEngineer = availableEngineers.Count > 0 ? availableEngineers[0] : null;
            //}
        }
    }
}
