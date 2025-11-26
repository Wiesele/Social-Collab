namespace MRO.SKM.GitProvider.Extensions;

public static class StringExtension
{
    public static string ToLocalBranchName(this string branchName)
    {
        if (branchName.ToLower().StartsWith("origin/".ToLower()))
        {
            return branchName.Substring("origin/".Length);
        }
        
        return branchName;
    }
}