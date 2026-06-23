namespace PolicySearchService.Services.AttachedPeopleImportService;

public interface IAttachedPeopleImportService
{
    Task<int> ImportFromCsvAsync(Stream csvStream, CancellationToken ct = default);
    Task<int> ImportFromArchiveAsync(Stream archiveStream, CancellationToken ct = default);
}
