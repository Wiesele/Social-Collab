using Microsoft.EntityFrameworkCore;
using MRO.SKM.Docu.ApplicationCore.Interfaces;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;

namespace MRO.SMK.Docu.ApplicationCore.Services;

public class CodeService
{
    private IEnumerable<ILanguageProviderService> LanguageProviderServices { get; set; }
    private IDatabaseContext Database { get; set; } 
    
    public CodeService(IEnumerable<ILanguageProviderService> languageProviderServices,
        IDatabaseContext database)
    {
        this.LanguageProviderServices = languageProviderServices;
        this.Database = database;
    }

    public List<Class> GetClassesInFile(string fileName)
    {
        var codeFile = this.Database.CodeFiles
            .Include(e => e.Classes)
            .ThenInclude(e => e.Methods)
            .FirstOrDefault(e => e.Key == fileName);

        if (codeFile == null)
        {
            return new List<Class>();
        }
      
        return codeFile.Classes;
    }

    public ILanguageProviderService GetLanguageproviderForFile(string path)
    {
        foreach (var item in this.LanguageProviderServices)
        {
            if (Path.GetExtension(path) == item.FileExtension)
            {
                return item;
            }
        }

        return null;
    }
}