namespace MRO.SKM.SDK.Interfaces;

public interface ILanguageModelService
{
    Task<string> GenerateSimpleContent(string prompt);
}