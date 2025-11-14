using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Editors;

namespace MRO.SKM.Docu.Models.DynamicEditors;

public interface IDynamicEditorValue
{
    BaseEditor EditorConfiguration { get; set; }
    string Name { get; set; }
    object GetValue();
}