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
}