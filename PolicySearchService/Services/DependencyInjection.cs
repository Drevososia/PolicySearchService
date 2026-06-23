using PolicySearchService.Data;
using PolicySearchService.DataReaders;
using PolicySearchService.Repositories.PersonRepository;

namespace PolicySearchService.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var mode = configuration["DeploymentMode:Mode"] ?? "Srz";

        switch (mode)
        {
            case "Srz":
                var srzConnection = configuration.GetConnectionString("SrzConnection")
                    ?? throw new InvalidOperationException("Connection string 'SrzConnection' not found.");
                services.AddSingleton<DapperDbContext>(new DapperDbContext(srzConnection));
                services.AddSingleton<IPersonRepository, PersonRepository>();
                break;

            case "AttachedPeople":
                var attachedConnection = configuration.GetConnectionString("AttachedPeopleConnection")
                    ?? throw new InvalidOperationException("Connection string 'AttachedPeopleConnection' not found.");
                services.AddSingleton<DapperDbContext>(new DapperDbContext(attachedConnection));
                services.AddSingleton<IPersonRepository, AttachedPeopleRepository>();
                break;

            default:
                throw new InvalidOperationException($"Неизвестный режим использования: {mode}. Используй 'Srz' или 'AttachedPeople'.");
        }

        services.AddSingleton<PersonSearchService.IPersonSearchService, PersonSearchService.PersonSearchService>();
        services.AddSingleton<AuthService.IAuthService, AuthService.AuthService>();
        services.AddSingleton<IDataReaderBuilder<Models.CSV.AttachedPerson>, AttachedPersonReaderBuilder>();

        var importConnection = configuration.GetConnectionString("AttachedPeopleConnection")
                               ?? configuration.GetConnectionString("SrzConnection")
                               ?? throw new InvalidOperationException("Не найдена строка подключения для импорта");
        services.AddSingleton<AttachedPeopleImportService.IAttachedPeopleImportService>(sp =>
            new AttachedPeopleImportService.AttachedPeopleImportService(
                new DapperDbContext(importConnection),
                sp.GetRequiredService<IDataReaderBuilder<Models.CSV.AttachedPerson>>(),
                sp.GetRequiredService<ILogger<AttachedPeopleImportService.AttachedPeopleImportService>>()));

        return services;
    }
}
