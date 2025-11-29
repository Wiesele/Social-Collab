using MRO.SKM.SDK.Interfaces;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class Class: CodeObject, ICommentable
{
    public List<Method> Methods { get; set; } = new List<Method>();
    public CodeFile CodeFile { get; set; }
    public string? Comment { get; set; }
}