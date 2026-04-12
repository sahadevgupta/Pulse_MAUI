using System;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services;

public class PriorityService(IDataManager dataManager) : IPriorityService
{

    /// <summary>
    /// Gets the priority list asynchronous.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Priority>> GetPriorityListAsync()
    {
        return await dataManager.GetAllPriority();
    }

    /// <summary>
    /// Gets the priority list for project asynchronous.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns></returns>
    public async Task<IEnumerable<Priority>> GetPriorityListForProjectAsync(int projectId)
    {
        IEnumerable<Priority> priorities = await GetPriorityListAsync();

        var list = priorities.ToList();

        // create a dummy blank/please select item
        Priority blank = new Priority();
        blank.PriorityId = -1;
        blank.ProjectId = projectId;
        blank.Value = "Please Select";

        list.Add(blank);

        return list
            .Where(p => p.ProjectId == projectId)
            .OrderBy(p => p.PriorityId);

    }

    /// <summary>
    /// Gets the priority identifier.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public async Task<int> GetPriorityId(int projectId, string value)
    {
        var availablePriorities = await GetPriorityListAsync();

        var priority = availablePriorities
            .ToList()
            .Where(p => p.ProjectId == projectId && p.Value == value)
            .FirstOrDefault(p => p.Value == value);

        return priority != null ? priority.PriorityId : 0;
    }

}


