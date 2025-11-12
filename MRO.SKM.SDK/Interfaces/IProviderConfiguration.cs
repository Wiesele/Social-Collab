using Microsoft.Extensions.DependencyInjection;

namespace MRO.SKM.SDK.Interfaces;

public interface IProviderConfiguration
{
    void RegisterServices(IServiceCollection services);
}    
