using System.Globalization;
using System.IO.Compression;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using FastMember;
using Microsoft.Data.SqlClient;
using PolicySearchService.Data;
using PolicySearchService.DataReaders;
using PolicySearchService.Models.CSV;

namespace PolicySearchService.Services.AttachedPeopleImportService;

public class AttachedPeopleImportService : IAttachedPeopleImportService
{
    private readonly DapperDbContext _context;
    private readonly IDataReaderBuilder<AttachedPerson> _readerBuilder;
    private readonly ILogger<AttachedPeopleImportService> _logger;

    public AttachedPeopleImportService(
        DapperDbContext context,
        IDataReaderBuilder<AttachedPerson> readerBuilder,
        ILogger<AttachedPeopleImportService> logger)
    {
        _context = context;
        _readerBuilder = readerBuilder;
        _logger = logger;
    }

    public async Task<int> ImportFromCsvAsync(Stream csvStream, CancellationToken ct = default)
    {
        var records = ParseCsv(csvStream);
        return await BulkInsertAsync(records, ct);
    }

    public async Task<int> ImportFromArchiveAsync(Stream archiveStream, CancellationToken ct = default)
    {
        using var archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
        foreach (var entry in archive.Entries)
        {
            if (!entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                continue;

            _logger.LogInformation("Обработка файла {FileName} из архива", entry.Name);
            await using var csvStream = entry.Open();
            var records = ParseCsv(csvStream);
            return await BulkInsertAsync(records, ct);
        }

        throw new InvalidOperationException("В архиве не найден CSV-файл");
    }

    private List<AttachedPerson> ParseCsv(Stream csvStream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            Encoding = Encoding.GetEncoding(1251),
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null
        };

        using var reader = new StreamReader(csvStream, config.Encoding);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<AttachedPersonMap>();

        var records = csv.GetRecords<AttachedPerson>().ToList();

        _logger.LogInformation("Прочитано {Count} записей из CSV", records.Count);
        return records;
    }

    private async Task<int> BulkInsertAsync(List<AttachedPerson> records, CancellationToken ct)
    {
        if (records.Count == 0)
        {
            _logger.LogWarning("Нет записей для вставки");
            return 0;
        }

        await using var connection = _context.CreateConnection();
        await connection.OpenAsync(ct);

        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync("TRUNCATE TABLE dbo.AttachedPeople", transaction: transaction);

            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction);
            bulkCopy.DestinationTableName = "dbo.AttachedPeople";
            bulkCopy.BatchSize = 5000;

            using var reader = _readerBuilder.BuildReader(records);
            await bulkCopy.WriteToServerAsync(reader, ct);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        _logger.LogInformation("Таблица dbo.AttachedPeople перезаписана: {Count} записей", records.Count);
        return records.Count;
    }


}
