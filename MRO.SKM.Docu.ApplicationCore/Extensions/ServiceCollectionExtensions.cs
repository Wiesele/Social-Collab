using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.GitProvider;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCore(this IServiceCollection services)
    {
        services.AddScoped<SettingService>();
        services.AddScoped<LoaderService>();
        services.AddScoped<RepositoryService>();
        services.AddScoped<RoslynService>();
    }

    public static void AddSDKResources(this IServiceCollection services)
    {
        // Dieser Code könnte Ausgelager werden um diese Services dynmisch zur laufzeit zu laden
        var gitProvider = new GitProvider();
        
        gitProvider.RegisterServices(services);
    }
}