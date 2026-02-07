using System;
using System.Threading.Tasks;

namespace Pulse_MAUI.Abstractions
{
	public interface ICloudService
	{
		ICloudTable<T> GetTable<T>() where T : TableData;
		Task<ICloudTable<T>> GetTableAsync<T>() where T : TableData;
	}
}
