using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.CSharpProvider;
using MRO.SKM.GitProvider;
using MRO.SKM.Google.Gemini;
using MRO.SMK.Docu.ApplicationCore.Services;

namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationCore(this IServiceCollection services)
    {
        services.AddScoped<SettingService>();
        services.AddScoped<LoaderService>();
        services.AddScoped<RepositoryService>();
        services.AddScoped<SourceControlService>();
        services.AddScoped<CodeService>();
        services.AddScoped<LanguageModelService>();
    }

    public static void AddSDKResources(this IServiceCollection services)
    {
        // Dieser Code könnte Ausgelager werden um diese Services dynmisch zur laufzeit zu laden
        var gitProvider = new GitProvider();
        gitProvider.RegisterServices(services);

        var csProvider = new CSharpProvider();
        csProvider.RegisterServices(services);
        
        var geminiProvider = new GeminiProvider();
        geminiProvider.RegisterServices(services);
    }

    public static IApplicationBuilder UseApplicationCore(this IApplicationBuilder app)
    {
        // app.UseMiddleware<GetRepoFromUrlMiddleware>();
        
        return app;
    }
}