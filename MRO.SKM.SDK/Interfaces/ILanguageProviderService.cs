using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ILanguageProviderService: IBaseProviderService
{
    Task<List<CodeFile>> AnalyzeRepository(Repository repository);
}