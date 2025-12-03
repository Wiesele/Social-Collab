using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MRO.SKM.SDK.Models.Comments;

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

    public static string GetElementText(this DocumentationCommentTriviaSyntax docTrivia, string localName)
    {
        var element = docTrivia.Content
            .OfType<XmlElementSyntax>()
            .FirstOrDefault(e => e.StartTag.Name.LocalName.Text == localName);

        if (element == null)
            return "";

        var text = element.GetElementContent();

        return text;
    }

    private static string GetElementContent(this XmlElementSyntax? element)
    {
        if (element == null)
        {
            return "";
        }

        var text = string.Concat(
            element.Content
                .OfType<XmlTextSyntax>()
                .SelectMany(t => t.TextTokens)
                .Select(t => t.Text));

        return text.Trim();
    }

    public static IEnumerable<CommentParameter> GetParams(this DocumentationCommentTriviaSyntax docTrivia)
    {
        var paramElements = docTrivia.Content
            .OfType<XmlElementSyntax>()
            .Where(e => e.StartTag.Name.LocalName.Text == "param");

        foreach (var el in paramElements)
        {
            var nameAttr = el.StartTag.Attributes
                .OfType<XmlNameAttributeSyntax>()
                .FirstOrDefault(a => a.Name.LocalName.Text == "name");

            var name = nameAttr?.Identifier?.Identifier.Text ?? "";

            var desc = el.GetElementContent();

            yield return new CommentParameter
            {
                Ref = name,
                Text = desc
            };
        }
    }

    public static IEnumerable<CommentException> GetExceptions(this DocumentationCommentTriviaSyntax docTrivia)
    {
        var exElements = docTrivia.Content
            .OfType<XmlElementSyntax>()
            .Where(e => e.StartTag.Name.LocalName.Text == "exception");

        foreach (var el in exElements)
        {
            var crefAttr = el.StartTag.Attributes
                .OfType<XmlCrefAttributeSyntax>()
                .FirstOrDefault();

            var cref = crefAttr?.Cref.ToString().Trim() ?? "";

            var desc = el.GetElementContent();

            yield return new CommentException
            {
                Ref = cref,
                Text = desc
            };
        }
    }
}