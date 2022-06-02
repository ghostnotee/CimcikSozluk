using CimcikSozluk.Api.Application.Interfaces.Repository;
using CimcikSozluk.Infrastructure.Persistence.Context;
using CimcikSozluk.Infrastructure.Persistence.Repositories;
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

        services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEntryCommentFavoriteRepository, EntryCommentFavoriteRepository>();
        services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();
        services.AddScoped<IEntryCommentVoteRepository, EntryCommentVoteRepository>();
        services.AddScoped<IEntryFavoriteRepository, EntryFavoriteRepository>();
        services.AddScoped<IEntryRepository, EntryRepository>();
        services.AddScoped<IEntryVoteRepository, EntryVoteRepository>();
        
        return services;
    }
}