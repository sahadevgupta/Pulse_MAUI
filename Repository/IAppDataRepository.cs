using System.Linq.Expressions;
using Seafarer.Models;
using FileInfo = Seafarer.Models.FileInfo;

namespace Pulse_MAUI.Repository;

/// <summary>
/// Generic repository service that delegates to the appropriate IDBAccessRepository
/// </summary>
public interface IAppDataRepository
{
    Task SaveCrewToDBAsync(Crew crew);
    Task SaveBookToDBAsync(Book book);
    Task SaveLicensesToDBAsync(IEnumerable<Licenses> licenses);
    Task SaveSQCToDBAsync(IEnumerable<Sqcs> sqcs);
    Task SaveSeaTimesoDBAsync(IEnumerable<SeaTime> seaTimes);
    Task SaveDocumentInfoAsync(FileInfo fileInfo);

    Task<Crew?> GetCrewAsync(string emailId);
    Task<Book?> GetBookInfoAsync(int crewId);
    Task<List<Licenses>> GetLicensesInfoAsync(int crewId);
    Task<List<Sqcs>> GetSQCInfoAsync(int crewId);
    Task<CrewPhoto?> GetCrewPhotoAsync(int crewId);
    Task<FileInfo?> GetDocumentInfoAsync(Expression<Func<FileInfo, bool>> predicate);
    Task<DocumentIssue> GetCrewDocumentsAsync(int crewId);
    Task<List<SeaTime>> GetSeaTimesAsync(int crewId);

    Task<int> DeleteAllAsync<T>() where T : new();
    
}
