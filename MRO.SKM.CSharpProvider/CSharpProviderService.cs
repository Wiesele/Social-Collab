using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.CSharpProvider;

public class CSharpProviderService: ILanguageProviderService
{
    public Guid UUID { get; } = new Guid("20C52B1F-9CB3-4D64-AC51-F17E0FA10579");
    public string DisplayName { get; } = "C#";
    
    
    public List<CodeFile> AnalyzeRepository(Repository repository)
    {
        var files = Directory.GetFiles(repository.Location, "*.cs", SearchOption.AllDirectories);

        List<CodeFile> methods = new List<CodeFile>();

        foreach (var file in files)
        {
            var data = this.AnalyzeFile(file);

            methods.Add(data);
        }

        return methods;
    }

    public CodeFile AnalyzeFile(string path)
    {
        var codeFile = new CodeFile()
        {
            Name = Path.GetFileNameWithoutExtension(path),
            Key = path,
            Classes = this.ExtractClasses(File.ReadAllText(path)),
        };
        
        return codeFile;
    }

    public List<Class> ExtractClasses(string sourceCode)
    {
        var data = new List<Class>();
        
        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var text = tree.GetText();

        var classes = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>();

        foreach (var item in classes)
        {
            var code = text.ToString(item.FullSpan);
            var key = item.Identifier.ToString();
            var name = item.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text;

            var itemData = new Class()
            {
                Name = name,
                Key = key,
                Body = code,
                Methods = this.ExtractMethod(code)
            };
            
            data.Add(itemData);
        }
        
        return data;
    }

    public List<Method> ExtractMethod(string sourceCode, bool includeLocalFunctions = false)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var text = tree.GetText();

        var results = new List<Method>();

        var methods = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var m in methods)
        {
            var code = text.ToString(m.FullSpan);
            var key = BuildKey(m);

            var type = m.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text ?? "";
            var parms = string.Join(",", m.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));

            results.Add(new()
            {
                Name = $"{type}({parms})",
                Key = key,
                Body = code,
            });
        }

        if (includeLocalFunctions)
        {
            var localFns = root.DescendantNodes()
                .OfType<LocalFunctionStatementSyntax>();
            foreach (var lf in localFns)
            {
                var code = text.ToString(lf.FullSpan);
                var key = BuildKey(lf);
                results.Add(new()
                {
                    Name = Guid.NewGuid().ToString(),
                    Key = key,
                    Body = code,
                });
            }
        }

        return results;
    }

    private string BuildKey(MethodDeclarationSyntax m)
    {
        var ns = m.FirstAncestorOrSelf<NamespaceDeclarationSyntax>()?.Name.ToString()
                 ?? m.FirstAncestorOrSelf<FileScopedNamespaceDeclarationSyntax>()?.Name.ToString()
                 ?? "";
        var type = m.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text ?? "";
        var parms = string.Join(",", m.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));
        return $"{ns}.{type}.{m.Identifier.Text}({parms})";
    }


    private string BuildKey(LocalFunctionStatementSyntax lf)
    {
        var container = lf.FirstAncestorOrSelf<MethodDeclarationSyntax>();
        var containerKey = container != null ? BuildKey(container) : "<top>";
        var parms = string.Join(",", lf.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));
        return $"{containerKey}::local {lf.Identifier.Text}({parms})";
    }
}