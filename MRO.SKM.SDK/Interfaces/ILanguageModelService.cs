using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.LanaugeModels;

namespace MRO.SKM.SDK.Interfaces;

public interface ILanguageModelService: IBaseProviderService
{
    Task<string> GenerateSimpleContent(string config, string prompt, string schema);
    Task<string> GenerateCodeGuide(string config, string prompt, int thinkingBudget, IEnumerable<UploadFile> files);
    LanguageModelDefaults GetDefaults();
    void GetEditorConfiguration(EditorConfiguration config);
}