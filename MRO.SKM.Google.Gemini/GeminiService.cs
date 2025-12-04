using Google.GenAI;
using MRO.SKM.SDK.Interfaces;

namespace MRO.SKM.Google.Gemini;

public class GeminiService: ILanguageModelService
{
    public async Task<string> GenerateSimpleContent(string prompt)
    {
        var client = new Client(apiKey:"");
        var response = client.Models.GenerateContentStreamAsync(
            model: "gemini-2.0-flash", contents: prompt
        );

        await foreach(var item in response)
        {
            
        }

        return "halloWelt";
    }
}