using Microsoft.Extensions.DependencyInjection;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCore(this IServiceCollection services)
    {
        services.AddScoped<SettingService>();
    }
}