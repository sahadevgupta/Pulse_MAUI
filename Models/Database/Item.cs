using System.ComponentModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Datasync.Client;

namespace Pulse_MAUI.Models.Database;

public class Item : DatasyncClientData
{

    public string name { get; set; }
    public int controlType { get; set; }
    public int recordID { get; set; }
    public string mimeType { get; set; }
    public int size { get; set; }
    public DateTime uploadTime { get; set; }
    public string uploadedBy { get; set; }
    public DateTime? lastUpdateTime { get; set; }
    public int? checkListStep { get; set; }
    public string description { get; set; }
    public string lastUpdatedBy { get; set; }
    public int projectId { get; set; }
    public string azurePath { get; set; }
    public string localPath { get; set; }
    public string localReferenceID { get; set; }

}