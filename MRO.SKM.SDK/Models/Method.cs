using MRO.SKM.SDK.Interfaces;
using MRO.SMK.SDK.Models.Abstracts;

namespace MRO.SKM.SDK.Models;

public class Method: CodeObject, ICommentable
{
    public string? Comment { get; set; }
    public Class Class { get; set; }
}