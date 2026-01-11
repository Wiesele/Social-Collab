# Wie finden wir <span class="r-fit-text">Methodendeklarationen?</span>

# NLP Parsing

<img src="/slides/images/image.png" class="image-custom"></img>

## Roslyn
``` csharp
private List<Method> ExtractMethod(string sourceCode)  
{  
    var tree = CSharpSyntaxTree.ParseText(sourceCode,  
            new CSharpParseOptions(LanguageVersion.Preview));  
    var root = (CompilationUnitSyntax)tree.GetRoot();  
  
    var methods = root.DescendantNodes()  
            .OfType<MethodDeclarationSyntax>();
}
```

``` csharp
private List<Class> ExtractClasses(string sourceCode)
{
    var data = new List<Class>();
    
    var tree = CSharpSyntaxTree.ParseText(sourceCode,
            new CSharpParseOptions(LanguageVersion.Preview));
    var root = (CompilationUnitSyntax)tree.GetRoot();
    
    var classes = root.DescendantNodes()
            .OfType<ClassDeclarationSyntax>();
}
```

``` csharp
private string? GetXmlDoc(SyntaxNode node)
{
    var docTrivia = node.GetLeadingTrivia()
                        .Select(t => t.GetStructure())
                        .OfType<DocumentationCommentTriviaSyntax>()
                        .FirstOrDefault();
        
    return docTrivia.ToFullString();
}
```