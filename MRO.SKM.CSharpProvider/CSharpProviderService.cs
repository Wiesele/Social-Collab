using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MRO.SKM.CSharpProvider.Extensions;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Interfaces;
using MRO.SKM.SDK.Models;
using MRO.SKM.SDK.Models.Comments;
using MRO.SMK.SDK.Models;

namespace MRO.SKM.CSharpProvider;

public class CSharpProviderService : ILanguageProviderService
{
    public Guid UUID { get; } = new Guid("20C52B1F-9CB3-4D64-AC51-F17E0FA10579");
    public string DisplayName { get; } = "C#";

    public string FileExtension { get; } = ".cs";


    public async Task<List<CodeFile>> AnalyzeRepository(Repository repository)
    {
        var files = Directory.GetFiles(repository.Location, "*" + this.FileExtension, SearchOption.AllDirectories);

        List<CodeFile> methods = new List<CodeFile>();

        foreach (var file in files)
        {
            var data = await this.AnalyzeFile(file);

            methods.Add(data);
        }

        return methods;
    }

    private async Task<CodeFile> AnalyzeFile(string path)
    {
        var codeFile = new CodeFile()
        {
            Name = Path.GetFileNameWithoutExtension(path),
            Key = path,
            Classes = this.ExtractClasses(await File.ReadAllTextAsync(path)),
        };

        return codeFile;
    }

    private List<Class> ExtractClasses(string sourceCode)
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
            var xmlDoc = this.GetXmlDoc(item);

            var code = text.ToString(item.FullSpan);
            var key = item.Identifier.ToString();
            var name = item.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text;

            var itemData = new Class()
            {
                Name = name,
                Key = item.BuildKey(),
                Body = code,
                Methods = this.ExtractMethod(code),
                Comment = xmlDoc,
                CanThrowException = false,
                HasParameters = false,
                HasReturnValue = false
            };

            data.Add(itemData);
        }

        return data;
    }

    private string? GetXmlDoc(SyntaxNode node)
    {
        var docTrivia = node.GetLeadingTrivia()
            .Select(t => t.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .FirstOrDefault();

        string? xmlDoc = docTrivia?.ToFullString() ?? "";

        if (xmlDoc.IsNullOrWhiteSpace())
        {
            xmlDoc = null;
        }

        return xmlDoc;
    }

    private List<Method> ExtractMethod(string sourceCode, bool includeLocalFunctions = false)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));
        var root = (CompilationUnitSyntax)tree.GetRoot();
        var text = tree.GetText();

        var results = new List<Method>();

        var methods = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        var properties = root.DescendantNodes()
            .OfType<PropertyDeclarationSyntax>();

        foreach (var m in methods)
        {
            var xmlDoc = this.GetXmlDoc(m);

            var code = text.ToString(m.FullSpan);
            var key = m.BuildKey();

            var parms = string.Join(",", m.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));

            var hasParams = m.ParameterList.Parameters.Any();

            results.Add(new()
            {
                Name = $"{m.Identifier.Text}({parms})",
                Key = key,
                Body = code,
                Comment = xmlDoc,
                CanThrowException = true,
                HasParameters = hasParams,
                HasReturnValue = m.ReturnType is not PredefinedTypeSyntax predefined ||
                                 predefined.Keyword.IsKind(SyntaxKind.VoidKeyword)
            });
        }

        foreach (var p in properties)
        {
            var xmlDoc = this.GetXmlDoc(p);

            var code = text.ToString(p.FullSpan);
            var key = p.BuildKey();

            results.Add(new()
            {
                Name = p.Identifier.Text,
                Key = key,
                Body = xmlDoc + code,
                Comment = xmlDoc,
                CanThrowException = false,
                HasParameters = false,
                HasReturnValue = false
            });
        }

        if (includeLocalFunctions)
        {
            var localFns = root.DescendantNodes()
                .OfType<LocalFunctionStatementSyntax>();
            foreach (var lf in localFns)
            {
                var code = text.ToString(lf.FullSpan);
                var key = lf.BuildKey();
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


    public async Task AddTriviaToMethod(string methodKey, string fileName, Comment comment)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Datei wurde nicht gefunden.", fileName);

        var sourceCode = await File.ReadAllTextAsync(fileName);


        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));

        var root = (CompilationUnitSyntax)await tree.GetRootAsync();

        var node = this.GetSyntaxNode(methodKey, root);

        await this.AddTriviaToNode(root, node, fileName, comment);
    }

    private SyntaxNode GetSyntaxNode(string methodKey, CompilationUnitSyntax root)
    {
        var method = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m => m.BuildKey() == methodKey);

        if (method != null)
        {
            return method;
        }

        var classElement = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(m => m.BuildKey() == methodKey);

        if (classElement != null)
        {
            return classElement;
        }

        var property = root.DescendantNodes()
            .OfType<PropertyDeclarationSyntax>()
            .FirstOrDefault(m => m.BuildKey() == methodKey);

        if (property != null)
        {
            return property;
        }

        throw new FileNotFoundException("Element " + methodKey + " wurde nicht gefunden.");
    }

    public async Task<string> GetObjectBody(string methodKey, string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Datei wurde nicht gefunden.", fileName);

        var sourceCode = await File.ReadAllTextAsync(fileName);

        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));

        var root = (CompilationUnitSyntax)await tree.GetRootAsync();

        var text = tree.GetText();

        var node = this.GetSyntaxNode(methodKey, root);

        var code = text.ToString(node.FullSpan);

        return code;
    }


    private async Task AddTriviaToNode(CompilationUnitSyntax root, 
        SyntaxNode method, 
        string fileName,
        Comment comment)
    {
        var methodIndent = method.GetLeadingTrivia()
            .ToFullString()
            .Split('\n')
            .LastOrDefault() ?? "";

        var docText = comment.GetTrivia(methodIndent);


        var newLeadingTrivia = SyntaxFactory.ParseLeadingTrivia(docText);

        var newMethod = method.WithLeadingTrivia(newLeadingTrivia);

        var newRoot = root.ReplaceNode(method, newMethod);

        await File.WriteAllTextAsync(fileName, newRoot.ToFullString());
    }

    private async Task<SyntaxTriviaList> GetXmlDoc(string methodKey, string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Datei wurde nicht gefunden.", fileName);

        var sourceCode = await File.ReadAllTextAsync(fileName);


        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));

        var root = (CompilationUnitSyntax)await tree.GetRootAsync();
    
        var node = this.GetSyntaxNode(methodKey, root);

        return node.GetLeadingTrivia();
    }

    public async Task<Comment> AnalyzeComment(string methodKey, string fileName)
    {
        var trivia = await GetXmlDoc(methodKey, fileName);

        var docTirvia = trivia.Select(t => t.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .FirstOrDefault();

        var paramsInCode = await this.GetParameterComments(methodKey, fileName);

        if (docTirvia == null)
        {
            foreach (var param in paramsInCode)
            {
                return new Comment()
                {
                    Params = paramsInCode
                };
            }            
        }

        var paramsInDoc = docTirvia.GetParams();

        foreach (var param in paramsInCode)
        {
            var paramInDoc = paramsInDoc.FirstOrDefault(e => e.Ref == param.Ref);

            if (paramInDoc != null)
            {
                param.Text = paramInDoc.Text;
            }
        }

        if (docTirvia != null)
        {

            return new Comment()
            {
                Summary = docTirvia.GetElementText("summary"),
                Returns = docTirvia.GetElementText("returns"),
                Params = paramsInCode,
                Exceptions = docTirvia.GetExceptions().ToList()
            };
        }
        else
        {
            return new Comment()
            {
                Params = paramsInCode
            };
        }
    }

    public async Task<List<CommentParameter>> GetParameterComments(string methodKey, string fileName)
    {
        var sourceCode = await File.ReadAllTextAsync(fileName);
        var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));

        var root = (CompilationUnitSyntax)await tree.GetRootAsync();

        var methods = root.DescendantNodes()
            .OfType<MethodDeclarationSyntax>();

        foreach (var method in methods)
        {
            var key = method.BuildKey();

            if (key != methodKey)
            {
                continue;
            }

            if (method != null && method.ParameterList.Parameters.Any())
            {
                var data = new List<CommentParameter>();
                foreach (var item in method.ParameterList.Parameters)
                {
                    var paramComment = new CommentParameter();
                    paramComment.DisplayName = item.Type.ToString() + " " + item.Identifier.ToString();
                    paramComment.Ref = item.Identifier.ToString();
                    data.Add(paramComment);
                }

                return data;
            }
            else
            {
                return new();
            }
        }


        return new();
    }
}