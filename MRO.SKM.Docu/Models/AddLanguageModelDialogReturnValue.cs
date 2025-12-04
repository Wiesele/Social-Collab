using MRO.SKM.Docu.Models.DynamicEditors;

namespace MRO.SKM.Docu.Models;

public class AddLanguageModelDialogReturnValue
{
    public Guid UUID { get; set; }
    public List<IDynamicEditorValue> Config { get; set; } 
}