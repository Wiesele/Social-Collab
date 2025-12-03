using System.Text;
using MRO.SKM.SDk.Extensions;
using MRO.SKM.SDK.Models.Comments;

namespace MRO.SKM.CSharpProvider.Extensions;

public static class CommentExtension
{
    /// <summary>
    ///  Das ist ein Ref.
    /// </summary>
    /// <param name="obj">Bla Bla</param>
    /// <exception cref="member">description</exception>
    /// <exception cref="wdawd">description</exception>
    /// <exception cref="meawdawdmber">description</exception>
    /// <returns>description</returns>
    public static string GetTrivia(this Comment comment, string indent)
    {
        var sb = new StringBuilder();

        if (!comment.Summary.IsNullOrWhiteSpace())
        {
            sb.AppendLine("<summary>");
            sb.AppendLine($"{comment.Summary}");
            sb.AppendLine("</summary>");
        }

        if (!comment.Returns.IsNullOrWhiteSpace())
        {
            sb.AppendLine("<returns>");
            sb.AppendLine($"{comment.Returns}");
            sb.AppendLine("</returns>");
        }

        foreach (var param in comment.Params.Where(e => !e.Text.IsNullOrWhiteSpace()))
        {
            if (param.Ref.IsNullOrWhiteSpace())
            {
                sb.AppendLine("<param>");
            }
            else
            {
                sb.AppendLine($"<param name=\"{param.Ref}\">");
            }

            sb.AppendLine($"{param.Text}");
            sb.AppendLine("</param>");
        }


        foreach (var exception in comment.Exceptions.Where(e => !e.Text.IsNullOrWhiteSpace()))
        {
            if (exception.Ref.IsNullOrWhiteSpace())
            {
                sb.AppendLine("<exception>");
            }
            else
            {
                sb.AppendLine($"<exception cref=\"{exception.Ref}\">");
            }

            sb.AppendLine($"{exception.Text}");
            sb.AppendLine("</exception>");
        }

        var commentXml = sb.ToString();

        if (commentXml.IsNullOrWhiteSpace())
        {
            return "";
        }

        var commentText = commentXml.XmlToComment(indent);

        return commentText;
    }

    private static string XmlToComment(this string xml, string indent)
    {
        var lines = xml.Trim().Replace("\r\n", "\n").Split('\n');

        var prefixed = lines
            .Select(line => indent + "/// " + line)
            .ToArray();

        var commentText = string.Join("\n", prefixed);


        return commentText + "\n" + indent;
    }
}