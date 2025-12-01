namespace MRO.SKM.SDK.Interfaces;


/// <summary>
///  Das ist ein Ref.
/// </summary>
/// <param name="obj">Bla Bla</param>
/// <exception cref="member">description</exception>
/// <exception cref="wdawd">description</exception>
/// <exception cref="meawdawdmber">description</exception>
/// <returns>description</returns>
public interface ICommentable
{
    string Key { get; set; }
    string? Comment { get; set; }
    string? Body { get; set; }
    
    bool HasReturnValue { get; set; }
    bool CanThrowException { get; set; }
    bool HasParameters { get; set; }
}
