using System;
using System.Collections.ObjectModel;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Interfaces;

public interface IEquipmentService
{
    Task<ObservableCollection<Equipment>> FetchEquipmentTasksAsync(int? projectId);
    Task<Equipment> GetEquipmentFromId(int? equipmentId);
}
