using System;
using System.Linq.Expressions;
using Pulse_MAUI.Constants;
using Pulse_MAUI.Models;
using SQLite;

namespace Pulse_MAUI.Repository;

public class DBAccessRepository<T> : IDBAccessRepository<T> where T : new()
{
    public SQLiteAsyncConnection SQLiteAsyncConnection { get; private set; }

    public async Task Init()
    {
        if (SQLiteAsyncConnection is not null)
            return;

        string? key = await SecureStorage.Default.GetAsync(PreferenceConstants.DatabaseToken);
        if (string.IsNullOrWhiteSpace(key))
        {
            await SecureStorage.Default.SetAsync(PreferenceConstants.DatabaseToken, Guid.NewGuid().ToString());
            key = await SecureStorage.Default.GetAsync(PreferenceConstants.DatabaseToken);
        }
        var options = new SQLiteConnectionString(DBConstants.DatabasePath, DBConstants.Flags, true, key, postKeyAction: c =>
        {
            c.Execute("PRAGMA cipher_compatibility = 3");
        });
        SQLiteAsyncConnection = new SQLiteAsyncConnection(options);

        await CreateTables();
    }

    public async Task InsertAsync(T item)
    {
        await Init();
        await SQLiteAsyncConnection.InsertAsync(item);
    }

    public async Task InsertOrReplaceAsync(T item)
    {
        await Init();
        await SQLiteAsyncConnection.InsertOrReplaceAsync(item);
    }

    public async Task InsertOrReplaceAllAsync(IEnumerable<T> items)
    {
        await Init();
        await SQLiteAsyncConnection.RunInTransactionAsync(conn =>
        {
            foreach (var item in items)
            {
                conn.InsertOrReplace(item);
            }
        });
    }

    public async Task InsertAllAsync(IEnumerable<T> items)
    {
        await Init();
        await SQLiteAsyncConnection.InsertAllAsync(items);
    }

    public async Task UpdateAsync(T item)
    {
        await Init();
        await SQLiteAsyncConnection.UpdateAsync(item);
    }

    public async Task UpdateAllAsync(IEnumerable<T> items)
    {
        await Init();
        await SQLiteAsyncConnection.UpdateAllAsync(items);
    }

    public async Task<int> DeleteAsync(T item)
    {
        await Init();
        return await SQLiteAsyncConnection.DeleteAsync(item);
    }

    public async Task<int> DeleteAllAsync()
    {
        await Init();
        return await SQLiteAsyncConnection.DeleteAllAsync<T>();
    }
    public async Task<List<T>> GetAllItemAsync()
    {
        await Init();
        return await SQLiteAsyncConnection.Table<T>().ToListAsync();
    }

    public async Task<List<T>> GetFilteredItemAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
    {
        await Init();
        var items = await SQLiteAsyncConnection.Table<T>().Where(predicate).ToListAsync();
        return  items;
    }
    private async Task CreateTables()
    {
        await CreateTableIfNotExists(nameof(Activity));
        await CreateTableIfNotExists(nameof(ActivityTask));
        await CreateTableIfNotExists(nameof(CommissioningSystem));
        await CreateTableIfNotExists(nameof(Component));
        await CreateTableIfNotExists(nameof(Discipline));
        await CreateTableIfNotExists(nameof(Engineer));
        await CreateTableIfNotExists(nameof(Equipment));
        await CreateTableIfNotExists(nameof(Item));
        await CreateTableIfNotExists(nameof(Lookup));
        await CreateTableIfNotExists(nameof(Priority));
        await CreateTableIfNotExists(nameof(Project));
        await CreateTableIfNotExists(nameof(PunchItem));
        await CreateTableIfNotExists(nameof(Unit));
        await CreateTableIfNotExists(nameof(User));
    }

    private async Task CreateTableIfNotExists(string tableName)
    {
        var info = await SQLiteAsyncConnection.GetTableInfoAsync(tableName);

        if (info == null || info.Count == 0)
        {
            await SQLiteAsyncConnection.CreateTableAsync<T>();
        }
    }
}
