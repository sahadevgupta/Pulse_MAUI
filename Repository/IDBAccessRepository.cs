using System;
using System.Linq.Expressions;
using SQLite;

namespace Pulse_MAUI.Repository;

public interface IDBAccessRepository<T> where T : new()
{
    SQLiteAsyncConnection SQLiteAsyncConnection {get;}
    Task<int> DeleteAllAsync();
    Task<int> DeleteAsync(T item);

    Task<List<T>> GetAllItemAsync();
    
    Task InsertAllAsync(IEnumerable<T> items);
    Task InsertOrReplaceAsync(T item);
    Task InsertOrReplaceAllAsync(IEnumerable<T> items);
    Task InsertAsync(T item);
    Task UpdateAllAsync(IEnumerable<T> items);
    Task UpdateAsync(T item);
    Task<List<T>> GetFilteredItemAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new();
}
