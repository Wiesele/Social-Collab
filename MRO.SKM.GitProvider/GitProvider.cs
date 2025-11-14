using Microsoft.Extensions.DependencyInjection;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.GitProvider;

public class GitProvider : IProviderConfiguration
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<GitNoAuthProviderServiceService>();
        services.AddSingleton<ISourceProviderService>(x => x.GetRequiredService<GitNoAuthProviderServiceService>());

        services.AddSingleton<GitHubSourceProviderService>();
        services.AddSingleton<ISourceProviderService>(x => x.GetRequiredService<GitHubSourceProviderService>());
    }
}