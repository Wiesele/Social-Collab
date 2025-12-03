using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MRO.SKM.CSharpProvider.Extensions;

public static class RoslynExtension
{
    
    public static string BuildKey(this MethodDeclarationSyntax m)
    {
        var type = m.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text ?? "";
        var parms = string.Join(",", m.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));
        return $"{type}.{m.Identifier.Text}({parms})";
    }

    public static string BuildKey(this PropertyDeclarationSyntax m)
    {
        var type = m.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text ?? "";
        return $"{type}.{m.Identifier.Text}";
    }


    public static string BuildKey(this LocalFunctionStatementSyntax lf)
    {
        var container = lf.FirstAncestorOrSelf<MethodDeclarationSyntax>();
        var containerKey = container != null ? BuildKey(container) : "<top>";
        var parms = string.Join(",", lf.ParameterList.Parameters.Select(p => p.Type?.ToString() ?? "?"));
        return $"{containerKey}::local {lf.Identifier.Text}({parms})";
    }

    public static string BuildKey(this ClassDeclarationSyntax lf)
    {
        var nameSpace = lf.GetFullNamespace();
        var name = lf.FirstAncestorOrSelf<TypeDeclarationSyntax>()?.Identifier.Text;
        
        return nameSpace + "." + name;
    }

    
    public static string GetFullNamespace(this SyntaxNode node)
    {
        var namespaces = new List<string>();

        foreach (var ns in node.Ancestors().OfType<BaseNamespaceDeclarationSyntax>())
            namespaces.Add(ns.Name.ToString());

        namespaces.Reverse(); // äußester Namespace zuerst

        return string.Join(".", namespaces);
    }
}