using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.Google.Gemini;

public class GeminiProvider: IProviderConfiguration
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<Gemini2FlashService>();
        services.AddScoped<Gemini25FlashService>();
        services.AddScoped<Gemini25FlashLightService>();
        services.AddScoped<ILanguageModelService>( e => e.GetRequiredService<Gemini2FlashService>());
        services.AddScoped<ILanguageModelService>( e => e.GetRequiredService<Gemini25FlashService>());
        services.AddScoped<ILanguageModelService>( e => e.GetRequiredService<Gemini25FlashLightService>());
    }
}