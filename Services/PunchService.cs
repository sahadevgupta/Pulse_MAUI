using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pulse_MAUI.Services
{
    public class PunchService(IDataManager dataManager, ILookupService lookupService) : IPunchService
    {
        /// <summary>
		/// The punches.
		/// </summary>
		public ObservableRangeCollection<PunchItem>? Punches { get; set; }

        /// <summary>
        /// Fetches the assigned punch list and populates the Punches collection.
        /// </summary>
        /// <returns>async task.</returns>
        public async Task FetchPunchListAsync()
        {
            Punches = new ObservableRangeCollection<PunchItem>();

            var availableUnits = await lookupService.GetUnitListAsync();
            var availableComSys = await lookupService.GetCommSystemListAsync();
            var availablePunches = await dataManager.GetAllPunchItemsAsync();
            var statusList = await lookupService.GetStatusLookups();


            foreach (var availablePunchItem in availablePunches)
            {

                availablePunchItem.CommissioningSystem = availableComSys.Where(c => c.PUCId == availablePunchItem.PUCId).Select(c => c.Name).FirstOrDefault() ?? string.Empty;
                availablePunchItem.Unit = availableUnits.Where(u => u.PUId == availablePunchItem.PUId).Select(u => u.Name).FirstOrDefault() ?? string.Empty;
                availablePunchItem.StatusString = statusList.Where(s => s.LookupId == availablePunchItem.Status).Select(s => s.Value).FirstOrDefault() ?? string.Empty;

                availablePunchItem.IsDirty = false;


                try
                {
                    if (availablePunchItem != null)
                    {
                        Punches.Add(availablePunchItem);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

            }

            //await FetchFilteredPunchList();
        }
        public async Task<IEnumerable<PunchItem>> GetPunchListAsync()
        {
            return await dataManager
                .GetAllPunchItemsAsync();
        }
    }
}
