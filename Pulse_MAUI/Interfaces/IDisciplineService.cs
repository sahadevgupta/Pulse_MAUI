using System;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IDisciplineService
{
    Task<IEnumerable<Discipline>> GetDisciplineListAsync();
    Task<int> GetDisciplineId(string name);
}
