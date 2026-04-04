using System.Diagnostics;
using System.Text.Json.Serialization;
using Pulse_MAUI.Helpers;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services
{
    public class PunchService(IDataManager dataManager,
        IEngineerService engineerService,
        ILookupService lookupService,
        IUserService userService) : IPunchService
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

                //availablePunchItem.IsDirty = false;


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

        /// <summary>
		/// Creates a new punchitem.
		/// </summary>
		/// <returns>The created punchitem.</returns>
		public async Task<PunchItem> CreatePunchItem()
        {
            var punchItem = new PunchItem();

            var projects = await dataManager
                .GetAllProjectsAsync();

            Project? defaultProject;
            if (projects != null && projects.Count() > 0)
                defaultProject = projects.ToList()[0];
            else
                defaultProject = null;

            punchItem.ProjectId = defaultProject != null ? defaultProject.ProjectId : 0;
            punchItem.PunchId = null;

            return punchItem;
        }

        /// <summary>
        /// Saves the punch item in the database.
        /// </summary>
        /// <returns>The punch item.</returns>
        /// <param name="punchItemToSave">Punch item to save.</param>
        public async Task SavePunchItem(PunchItem punchItemToSave)
        {
            if (string.IsNullOrEmpty(punchItemToSave.Id))
            {
                punchItemToSave.CreatedBy = userService.CurrentUser?.ApexId;
                punchItemToSave.CreatedOn = DateTime.Now;
            }

            // This should no be set as only the UI can assign activites
            punchItemToSave.AssignedToEngineer = engineerService.CurrentEngineer.EngineerId;

            punchItemToSave.UpdatedBy = userService.CurrentUser?.ApexId;
            punchItemToSave.UpdatedOn = DateTime.Now;

            await dataManager.SavePunchItemAsync(punchItemToSave);

        }
    }
}
