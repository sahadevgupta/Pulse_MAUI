using System;
using System.Collections.Generic;
using System.Text;

namespace Pulse_MAUI.Interfaces
{
    public interface ISecureStorageService
    {
        Task<T> GetAsync<T>(string key);
        bool Remove(string key);
        void RemoveAll();
        Task SetAsync(string key, string value);
    }
}
