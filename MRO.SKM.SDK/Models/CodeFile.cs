using MRO.SMK.SDK.Models;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class CodeFile: CodeObject
{
    public CodeFile()
    {
        this.Classes = new();
    }
    
    public List<Class> Classes { get; set; }
    public Repository Repository { get; set; }
    public Guid RepositoryId { get; set; }
}