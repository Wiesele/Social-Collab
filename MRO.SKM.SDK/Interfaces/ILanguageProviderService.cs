using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Comments;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ILanguageProviderService: IBaseProviderService
{
    Task<List<CodeFile>> AnalyzeRepository(Repository repository);
    List<CommentParameter> GetParameterComments(string methodKey, string fileName);
    string FileExtension { get; }
}