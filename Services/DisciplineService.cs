using System;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services;

public class DisciplineService(IDataManager dataManager) : IDisciplineService
{
    // <summary>
    /// Gets the discipline list asynchronous.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Discipline>> GetDisciplineListAsync()
    {
        return await dataManager.GetAllDisciplines();
    }

    /// <summary>
    /// Gets the discipline identifier.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <returns></returns>
    public async Task<int> GetDisciplineId(string name)
    {
        var availableDisciplines = await GetDisciplineListAsync();

        var discipline = availableDisciplines
            .ToList()
            .Where(p => p.Name == name)
            .FirstOrDefault(p => p.Name == name);

        if (discipline != null)
        {
            if (discipline.DisciplineId != null)
            {
                return (int)discipline.DisciplineId;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

}
