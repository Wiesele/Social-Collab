using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ISourceProviderService: IBaseProviderService
{
    void GetEditorConfiguration(EditorConfiguration config);
    Task CloneRepository(Repository repository);  
}