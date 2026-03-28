using System;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IPriorityService
{
    Task<IEnumerable<Priority>> GetPriorityListAsync();
    Task<IEnumerable<Priority>> GetPriorityListForProjectAsync(int projectId);
}
