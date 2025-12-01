using MRO.SKM.SDK.Interfaces;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class Class: CodeObject, ICommentable
{
    public Class()
    {
        this.Methods = new();
    }
    
    public List<Method> Methods { get; set; }
    public CodeFile CodeFile { get; set; }
    public string? Comment { get; set; }
    public bool HasReturnValue { get; set; }
    public bool CanThrowException { get; set; }
    public bool HasParameters { get; set; }
}