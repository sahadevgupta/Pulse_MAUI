using System;
using System.Collections.ObjectModel;
using Pulse_MAUI.Interfaces;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Services;

public class EquipmentService(IDataManager dataManager) : IEquipmentService
{
    /// <summary>
    /// Gets the equipment from identifier.
    /// </summary>
    /// <param name="equipmentId">The equipment identifier.</param>
    /// <returns></returns>
    public async Task<Equipment> GetEquipmentFromId(int? equipmentId)
    {
        IEnumerable<Equipment> allEquipment = await dataManager.GetAllEquipmentAsync();

        var availableEquipment = allEquipment.ToList();
        Equipment equipmentNone = new Models.Equipment();
        equipmentNone.Name = "None";
        equipmentNone.EquipmentId = 0;
        availableEquipment.Add(equipmentNone);


        Equipment equip = new Equipment();

        if (equipmentId != null)
        {
            equip = availableEquipment.FirstOrDefault(e => e.EquipmentId == equipmentId);
        }
        else
        {
            equip = availableEquipment.FirstOrDefault(e => e.EquipmentId == 0);
        }

        return equip;
    }

    /// <summary>
    /// Fetches the equipment tasks asynchronous.
    /// </summary>
    /// <param name="activity">The activity.</param>
    /// <returns></returns>
    public async Task<ObservableCollection<Equipment>> FetchEquipmentTasksAsync(int? projectId)
    {
        var equipmentList = await dataManager.GetAllEquipmentAsync();

        var availableEquipment = equipmentList.Where(a => a.ActiveProject == projectId).OrderBy(e => e.Name).ToList();

        Equipment equipmentNone = new Models.Equipment();
        equipmentNone.Name = "None";
        equipmentNone.EquipmentId = 0;
        availableEquipment.Add(equipmentNone);

        return new ObservableCollection<Equipment>(availableEquipment); ;
    }
}
