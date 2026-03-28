using Pulse_MAUI.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Interfaces
{
    public interface ISyncServiceApi
    {
        [Get("/tables/Activity")]
        Task<IEnumerable<Activity>> GetActivitiesAsync([HeaderCollection] IDictionary<string, string> headers, [Query]int skip = 0, [Query] int top = 50, [Query] bool includeDeleted = true);
    }
}
