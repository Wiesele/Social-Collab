using MRO.SKM.Docu.Models.DynamicEditors;

namespace MRO.SKM.Docu.Extensions;

public static class ListExtension
{
    public static Dictionary<string, object> ToDictionary(this List<IDynamicEditorValue> obj)
    {
        var data = new Dictionary<string, object>();

        foreach (var item in obj)
        {
            data.Add(item.Name, item.GetValue());
        }
        
        return data;
    }
}