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
        /// Gets the specific lookup values for Status.
        /// </summary>
        /// <returns>The status lookups.</returns>
        public async Task<IEnumerable<Lookup>> GetActivityStatusLookups()
        {
            var availableLookups = await GetLookupListAsync();

            var activitystatusLookups = availableLookups
                .ToList()
                .Where(p => p.Name == activityStatus);

            return activitystatusLookups;
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

        /// <summary>
		/// Gets the component list async.
		/// </summary>
		/// <returns>The component list async.</returns>
		public async Task<IEnumerable<Component>> GetComponentListAsync()
        {
            return await dataManager
                .GetAllComponentsAsync();
        }

        /// <summary>
        /// Gets the activity status lookups.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Lookup>> GetActivityTaskStatusLookups()
        {
            var StatusList = new List<Lookup>();

            var NotSet = new Lookup();
            NotSet.LookupId = 0;
            NotSet.ListOrder = 0;
            NotSet.Value = "---";

            var Pass = new Lookup();
            Pass.LookupId = 1;
            Pass.ListOrder = 1;
            Pass.Value = "Pass";

            var Fail = new Lookup();
            Fail.LookupId = 2;
            Fail.ListOrder = 2;
            Fail.Value = "Fail";

            var NA = new Lookup();
            NA.LookupId = 3;
            NA.ListOrder = 3;
            NA.Value = "N/A";

            var PL = new Lookup();
            PL.LookupId = 4;
            PL.ListOrder = 4;
            PL.Value = "P/L";

            StatusList.Add(NotSet);
            StatusList.Add(Pass);
            // Dont Add Fail to the options list
            //StatusList.Add(Fail);
            StatusList.Add(NA);
            StatusList.Add(PL);

            return StatusList;

        }

        /// <summary>
		/// Returns the Identifier of a lookup.
		/// </summary>
		/// <returns>The status lookup identifier.</returns>
		/// <param name="value">Value.</param>
		public async Task<int> GetStatusLookupId(string value)
        {
            var availableLookups = await GetLookupListAsync();

            var lookup = availableLookups
                .ToList()
                .Where(p => p.Name == statusLookupName)
                .FirstOrDefault(p => p.Value == value);

            return lookup != null ? lookup.LookupId : 0;
        }

        /// <summary>
		/// Returns the Identifier of a lookup.
		/// </summary>
		/// <returns>The status lookup identifier.</returns>
		/// <param name="value">Value.</param>
		public async Task<int> GetActivityTaskStatusLookupId(string value)
        {

            IEnumerable<Lookup> availableLookups = await GetActivityTaskStatusLookups();

            Lookup lookup = availableLookups
                .ToList()
                .Where(p => p.Value == value).FirstOrDefault();

            return lookup != null ? lookup.LookupId : 0;
        }

        /// <summary>
        /// Gets the activity status lookup identifier.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task<int> GetActivityStatusLookupId(string value)
        {
            var availableLookups = await GetLookupListAsync();

            var lookup = availableLookups
                .ToList()
                .Where(p => p.Name == activityStatus)
                .FirstOrDefault(p => p.Value == value);

            return lookup != null ? lookup.LookupId : 0;
        }

        /// <summary>
        /// Returns the value field of a lookup.
        /// </summary>
        /// <returns>The lookup value.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<string?> GetLookupValue(int id)
        {
            var availableLookups = await GetLookupListAsync();

            var lookup = availableLookups
                .ToList()
                .FirstOrDefault(p => p.LookupId == id);

            return lookup != null ? lookup.Value : string.Empty;
        }

        /// <summary>
        /// Gets the activty list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Activity>> GetActivtyListAsync()
        {
            return await dataManager
                .GetAllActivitiesAsync();
        }
    }
}
