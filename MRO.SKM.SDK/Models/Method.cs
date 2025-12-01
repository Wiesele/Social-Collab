using MRO.SKM.SDK.Interfaces;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class Method: CodeObject, ICommentable
{
    public string? Comment { get; set; }
    public bool HasReturnValue { get; set; }
    public bool CanThrowException { get; set; }
    public bool HasParameters { get; set; }
    public Class Class { get; set; }
}