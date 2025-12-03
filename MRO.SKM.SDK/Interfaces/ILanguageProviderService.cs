using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Comments;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ILanguageProviderService: IBaseProviderService
{
    Task<List<CodeFile>> AnalyzeRepository(Repository repository);
    string FileExtension { get; }
    Task AddTriviaToMethod(string methodKey, string fileName, Comment comment);
    Task<string> GetObjectBody(string methodKey, string fileName);
    Task<Comment> AnalyzeComment(string methodKey, string fileName);
}