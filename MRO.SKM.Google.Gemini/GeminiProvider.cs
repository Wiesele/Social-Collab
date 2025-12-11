using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.Google.Gemini;

public class GeminiProvider: IProviderConfiguration
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<Gemini2FlashService>();
        services.AddSingleton<Gemini25FlashService>();
        services.AddSingleton<Gemini25FlashLightService>();
        services.AddSingleton<ILanguageModelService>( e => e.GetRequiredService<Gemini2FlashService>());
        services.AddSingleton<ILanguageModelService>( e => e.GetRequiredService<Gemini25FlashService>());
        services.AddSingleton<ILanguageModelService>( e => e.GetRequiredService<Gemini25FlashLightService>());
    }
}