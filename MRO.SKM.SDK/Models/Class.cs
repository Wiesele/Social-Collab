using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class Class: CodeObject
{
    public List<Method> Methods { get; set; } = new List<Method>();
    public CodeFile CodeFile { get; set; }
}