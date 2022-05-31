using CimcikSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CimcikSozluk.Infrastructure.Persistence.Extensions;

public static class Registration
{
    public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CimcikSozlukContext>(conf =>
        {
            var connStr = configuration.GetConnectionString("SqlServerExpress");
            conf.UseSqlServer(connStr, opt => { opt.EnableRetryOnFailure(); });
        });

        var seedData = new SeedData();
        seedData.SeedAsync(configuration).GetAwaiter().GetResult();
        
        return services;
    }
}