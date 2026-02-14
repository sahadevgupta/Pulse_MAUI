using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class LookupService(IDataManager dataManager) : ILookupService
    {
        private const string statusLookupName = "Status";
        private const string activityTaskStatus = "NullablePassFail";
        private const string controlTypeLookupName = "ControlType";
        private const string activityStatus = "ActivityStatus";

        /// <summary>
        /// Gets the specific lookup values for Control Types.
        /// </summary>
        /// <returns>The status lookups.</returns>
        public async Task<IEnumerable<Lookup>> GetControlTypeLookups()
        {
            var availableLookups = await GetLookupListAsync();

            var controlTypeLookups = availableLookups
                .ToList()
                .Where(p => p.Name == controlTypeLookupName);

            return controlTypeLookups;
        }

        /// <summary>
        /// Get the values from the look up table async.
        /// </summary>
        /// <returns>The lookup list async.</returns>
        public async Task<IEnumerable<Lookup>> GetLookupListAsync()
        {
            return await dataManager
                .GetAllLookupsAsync();
        }

        /// <summary>
		/// Gets the projects async.
		/// </summary>
		/// <returns>The project list async.</returns>
		public async Task<IEnumerable<Project>> GetProjectListAsync()
        {
            return await dataManager
                .GetAllProjectsAsync();
        }

        /// <summary>
        /// Gets the specific lookup values for Status.
        /// </summary>
        /// <returns>The status lookups.</returns>
        public async Task<IEnumerable<Lookup>> GetStatusLookups()
        {
            var availableLookups = await GetLookupListAsync();

            var statusLookups = availableLookups
                .ToList()
                .Where(p => p.Name == statusLookupName);

            return statusLookups;
        }

        /// <summary>
        /// Gets the unit list async.
        /// </summary>
        /// <returns>The unit list async.</returns>
        public async Task<IEnumerable<Unit>> GetUnitListAsync()
        {
            return await dataManager
                .GetAllUnitsAsync();
        }

        /// <summary>
        /// Gets the commissioning systems async.
        /// </summary>
        /// <returns>The comm system list async.</returns>
        public async Task<IEnumerable<CommissioningSystem>> GetCommSystemListAsync()
        {
            return await dataManager
                .GetAllCommissioningSystemsAsync();
        }
    }
}
