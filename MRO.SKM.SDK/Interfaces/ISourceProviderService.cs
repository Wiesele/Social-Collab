using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.SourceControl;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ISourceProviderService: IBaseProviderService
{
    void GetEditorConfiguration(EditorConfiguration config);
    Task CloneRepository(Repository repository);  
    Task<List<Branch>> ListBranches(Repository repository);
    Task ChangeBranch(Repository repository, Branch branch);
    Task UpdateRepository(Repository repository);
}