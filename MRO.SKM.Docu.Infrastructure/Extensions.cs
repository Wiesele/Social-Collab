using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MRO.SKM.Docu.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connectionString)
    {
        collection.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionString));
        collection.AddScoped<IDatabaseContext>(p => p.GetService<DatabaseContext>());
        return collection;
    }
}