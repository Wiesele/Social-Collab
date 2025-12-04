using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.Google.Gemini;

public class GeminiProvider: IProviderConfiguration
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<GeminiService>();
        services.AddSingleton<ILanguageModelService>( e => e.GetRequiredService<GeminiService>());
    }
}