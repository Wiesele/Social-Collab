using MRO.SMK.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class CodeFile: CodeObject
{
    public List<Class> Classes { get; set; } = new List<Class>();
    public Repository Repository { get; set; }
}