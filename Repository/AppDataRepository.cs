using System.Linq.Expressions;

namespace Pulse_MAUI.Repository;

/// <summary>
/// Repository service that dynamically resolves repositories from the DI container
/// Injects only IServiceProvider instead of multiple generic repository instances
/// </summary>
public partial class AppDataRepository(IServiceProvider serviceProvider) : IAppDataRepository
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Save Crew to db
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task SaveCrewToDBAsync(Crew crew)
    {
        // Delete Crew Address & Crew
        await DeleteAsync(crew.CrewAddress);
        await DeleteAsync(crew);

        await InsertOrReplaceAsync(crew);
        
        crew.CrewAddress?.CrewId = crew.Id;
        await InsertOrReplaceAsync(crew.CrewAddress);
    }

    /// <summary>
    /// Save Book into to DB
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public async Task SaveBookToDBAsync(Book book)
    {
        await DeleteAsync(book);
        await InsertOrReplaceAsync(book);
    }

    /// <summary>
    /// Save Licenses into to DB
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public async Task SaveLicensesToDBAsync(IEnumerable<Licenses> licenses)
    {
        await DeleteAllAsync<Licenses>();
        await InsertOrReplaceAllAsync(licenses);
    }

    /// <summary>
    /// Save SQcs into to DB
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public async Task SaveSQCToDBAsync(IEnumerable<Sqcs> sqcs)
    {
        await DeleteAllAsync<Sqcs>();
        await InsertOrReplaceAllAsync(sqcs);
    }

    /// <summary>
    /// Save sea times to DB
    /// </summary>
    /// <param name="seaTimes"></param>
    /// <returns></returns>
    public async Task SaveSeaTimesoDBAsync(IEnumerable<SeaTime> seaTimes)
    {
        await DeleteAllAsync<SeaTime>();
        await InsertOrReplaceAllAsync(seaTimes);
    }

    /// <summary>
    /// Save document to DB
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public async Task SaveDocumentInfoAsync(FileInfo fileInfo)
    {
        var fileExist = await GetDocumentInfoAsync(x => x.FileId == fileInfo.FileId && x.DocumentId == fileInfo.DocumentId && x.DocumentType == fileInfo.DocumentType);
        if (fileExist is not null)
        {
            await DeleteAsync(fileExist);

        }
        await InsertAsync(fileInfo);
    }

    /// <summary>
    /// Get Crew photo from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns></returns>
    public async Task<CrewPhoto?> GetCrewPhotoAsync(int crewId)
    {
        var crewRepo = GetRepository<Crew>();
        var crew = await crewRepo.GetFilteredItemAsync<Crew>(x => x.Id == crewId);
        return crew?.FirstOrDefault()?.CrewPhoto;
    }

    /// <summary>
    /// Get Crew detials from DB
    /// </summary>
    /// <param name="emailId"></param>
    /// <returns></returns>
    public async Task<Crew?> GetCrewAsync(string emailId)
    {
        var crewRepo = GetRepository<Crew>();
        var addressRepo = GetRepository<CrewAddress>();
        var records = await crewRepo.GetFilteredItemAsync<Crew>(x => x.Email == emailId);
        Crew? crew = records.FirstOrDefault();
        if(crew is not null)
        {
            var addresses = await crewRepo.GetFilteredItemAsync<CrewAddress>(a => a.CrewId == crew.Id);
            crew.CrewAddress = addresses.FirstOrDefault();
        }
        return crew;
    }

    /// <summary>
    /// Get Book details from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns></returns>
    public async Task<Book?> GetBookInfoAsync(int crewId)
    {
        var bookRepo = GetRepository<Book>();
        var records = await bookRepo.GetFilteredItemAsync<Book>(x => x.CrewId == crewId);
        return records?.FirstOrDefault();
    }

    /// <summary>
    /// Get Licenses from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns></returns>
    public async Task<List<Licenses>> GetLicensesInfoAsync(int crewId)
    {
        var licenseRepo = GetRepository<Licenses>();
        return await licenseRepo.GetFilteredItemAsync<Licenses>(x => x.CrewId == crewId);
    }

    /// <summary>
    /// Get Sqcs from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns></returns>
    public async Task<List<Sqcs>> GetSQCInfoAsync(int crewId)
    {
        var sqcRepo = GetRepository<Sqcs>();
        return await sqcRepo.GetFilteredItemAsync<Sqcs>(x => x.CrewId == crewId);
    }

    /// <summary>
    /// Get Document details from DB
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<FileInfo?> GetDocumentInfoAsync(Expression<Func<FileInfo, bool>> predicate)
    {
        var fileRepo = GetRepository<FileInfo>();
        var items = await fileRepo.GetFilteredItemAsync(predicate);
        return items.FirstOrDefault();
    }

    /// <summary>
    /// Get Crew documets from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns>Object of DocumentIssue</returns>
    public async Task<DocumentIssue> GetCrewDocumentsAsync(int crewId)
    {
        var bookRepo = GetRepository<Book>();
        var licenseRepo = GetRepository<Licenses>();
        var sqcRepo = GetRepository<Sqcs>();

        var books = await bookRepo.GetFilteredItemAsync<Book>(b => b.CrewId == crewId);
        var licenses = await licenseRepo.GetFilteredItemAsync<Licenses>(l => l.CrewId == crewId);
        var sqcs = await sqcRepo.GetFilteredItemAsync<Sqcs>(s => s.CrewId == crewId);

        return new DocumentIssue
        {
            Book = books.FirstOrDefault(),
            Sqcs = sqcs,
            Licenses = licenses
        };
    }

    /// <summary>
    /// Get Sea times from DB
    /// </summary>
    /// <param name="crewId"></param>
    /// <returns>Object of DocumentIssue</returns>
    public async Task<List<SeaTime>> GetSeaTimesAsync(int crewId)
    {
        var seaTimeRepo = GetRepository<SeaTime>();
        return await seaTimeRepo.GetFilteredItemAsync<SeaTime>(s => s.CrewId == crewId);
    }

    public async Task<int> DeleteAllAsync<T>() where T : new()
    {
        var repo = GetRepository<T>();
        return await repo.DeleteAllAsync();
    }



    private async Task InsertOrReplaceAsync<T>(T item) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.InsertOrReplaceAsync(item);
    }

    private async Task InsertAsync<T>(T item) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.InsertAsync(item);
    }

    private async Task InsertOrReplaceAllAsync<T>(IEnumerable<T> items) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.InsertOrReplaceAllAsync(items);
    }

    private async Task InsertAllAsync<T>(IEnumerable<T> items) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.InsertAllAsync(items);
    }

    private async Task UpdateAsync<T>(T item) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.UpdateAsync(item);
    }

    private async Task UpdateAllAsync<T>(IEnumerable<T> items) where T : new()
    {
        var repo = GetRepository<T>();
        await repo.UpdateAllAsync(items);
    }

    private async Task<int> DeleteAsync<T>(T item) where T : new()
    {
        var repo = GetRepository<T>();
        return await repo.DeleteAsync(item);
    }

    

    private async Task<List<T>> GetAllItemAsync<T>() where T : new()
    {
        var repo = GetRepository<T>();
        return await repo.GetAllItemAsync();
    }

    /// <summary>
    /// Dynamically resolves the correct repository from the service provider
    /// No hard-coded if-else statements or manual mapping needed
    /// </summary>
    private IDBAccessRepository<T> GetRepository<T>() where T : new()
    {
        var repositoryType = typeof(IDBAccessRepository<>).MakeGenericType(typeof(T));
        var repo = _serviceProvider.GetService(repositoryType) as IDBAccessRepository<T>;

        if (repo == null)
            throw new InvalidOperationException($"Repository not configured for type '{typeof(T).Name}'");

        return repo;
    }
}
