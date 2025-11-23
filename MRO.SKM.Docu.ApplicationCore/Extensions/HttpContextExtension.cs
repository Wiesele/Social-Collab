using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class HttpContextExtension
{
    public static Guid GetRepositoryId(this HttpContext context)
    {
        var currentPath = context.Request.Path;

        // Kleiner Hack - Am Ende des Pfades ein Whitespace einfügen um im Regex zu matchen
        // Demo: https://regexr.com/8icsn
        var regex = "\\/repository\\/(.+?)[\\/|\\s]";
        var match = Regex.Match(currentPath + " ", regex);

        if (match.Success)
        {
            var guid = match.Groups[1].Value;

            return Guid.Parse(guid);
        }

        return Guid.Empty;
    }
}