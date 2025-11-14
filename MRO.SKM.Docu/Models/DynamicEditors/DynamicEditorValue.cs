using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Editors;

namespace MRO.SKM.Docu.Models.DynamicEditors;

public class DynamicEditorValue<T>: IDynamicEditorValue
{
    public T Value { get; set; }
    public BaseEditor EditorConfiguration { get; set; }
    public string Name { get; set; }
    
    public object GetValue()
    {
        return this.Value;
    }
}