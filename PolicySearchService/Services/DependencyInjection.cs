using PolicySearchService.Data;
using PolicySearchService.Repositories;

namespace PolicySearchService.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddSingleton(new SrzDbContext(connection));

        services.AddSingleton<IPersonRepository>(sp =>
            new PersonRepository(sp.GetRequiredService<SrzDbContext>()));

        services.AddSingleton<IPersonSearchService, PersonSearchService>();
        services.AddSingleton<IAuthService, AuthService>();

        return services;
    }
}
