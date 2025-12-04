using MRO.SKM.SDK.Interfaces;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class LanguageModelService
{
    private IEnumerable<ILanguageModelService> LanugageModels { get; set; }
    public LanguageModelService(IEnumerable<ILanguageModelService> languageModels)
    {
        this.LanugageModels = languageModels;
    }

    public async Task<string> GenerateDocumentation(ICommentable classOrModel, string fileContents)
    {
        var model = this.LanugageModels.First();

        var prompt = "Du bist ein Senior Programmierer. \n" +
                     "Deine Aufgabe ist es detailierte Kommentare für Code zu erstellen. \n" +
                     "Erstelle eine Beschreibung für die folgenden Codeausschnit:\n\n" + classOrModel.Body;
        
        return await model.GenerateSimpleContent(prompt);
    }
}