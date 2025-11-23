using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.CSharpProvider;

public class CSharpProvider: IProviderConfiguration
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<CSharpProviderService>();
        services.AddSingleton<ILanguageProviderService>( e => e.GetRequiredService<CSharpProviderService>());
    }
}