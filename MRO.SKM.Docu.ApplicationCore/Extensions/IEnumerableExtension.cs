using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class IEnumerableExtension
{
    public static ILanguageProviderService GetLanguageProvider(
        this IEnumerable<ILanguageProviderService> providers, RepositoryLanguage language)
    {
        return providers.FirstOrDefault(e => e.UUID == language.ProviderId);
    }

    public static ISourceProviderService GetSourceProvider(
        this IEnumerable<ISourceProviderService> providers, Repository repository)
    {
        return providers.First(e => e.UUID == repository.SourceProviderService);
    }
    
    public static ISourceProviderService GetSourceProvider(
        this IEnumerable<ISourceProviderService> providers, Guid sourceProviderId)
    {
        return providers.First(e => e.UUID == sourceProviderId);
    }
}