namespace MRO.SMK.SDK.Models.Abstracts;

public abstract class CodeObject: NamedEntity
{
    public string Key { get; set; }
    public string Body { get; set; }
}