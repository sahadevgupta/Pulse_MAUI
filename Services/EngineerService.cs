using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services
{
    public class EngineerService(IDataManager dataManager) : IEngineerService
    {

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
            var engineers = await dataManager.GetAllEngineersAsync();

            if (engineers != null)
            {
                var availableEngineers = engineers.ToList();

                CurrentEngineer = availableEngineers.Count > 0 ? availableEngineers[0] : null;
            }
        }
    }
}
