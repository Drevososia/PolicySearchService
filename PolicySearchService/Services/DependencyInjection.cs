using PolicySearchService.Data;
using PolicySearchService.Repositories;

namespace PolicySearchService.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var mainConnection = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        var archiveConnection = configuration.GetConnectionString("ArchiveConnection")
            ?? throw new InvalidOperationException("Connection string 'ArchiveConnection' not found.");

        services.AddSingleton(new SrzDbContext(mainConnection));
        services.AddSingleton(new AttachedPersonDbContext(archiveConnection));

        services.AddSingleton<IPersonRepository>(sp =>
            new PersonRepository(sp.GetRequiredService<AttachedPersonDbContext>()));
        
        services.AddSingleton<IPersonRepository>(sp =>
            new PersonRepository(sp.GetRequiredService<SrzDbContext>()));

        services.AddSingleton<IPersonSearchService, PersonSearchService>();
        services.AddSingleton<IAuthService, AuthService>();

        return services;
    }
}
