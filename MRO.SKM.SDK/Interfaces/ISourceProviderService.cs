using MRO.SKM.SDK.Models;

namespace MRO.SKM.SDK.Interfaces;

public interface ISourceProviderService: IBaseProviderService
{
    public void GetEditorConfiguration(EditorConfiguration config);
}