namespace MRO.SMK.Docu.ApplicationCore.Extensions;

public static class StringExtension
{
    public static string AsJson<T>(this T obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj);
    }
    
    public static T ParseAsJson<T>(this string str)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(str);
    }
}