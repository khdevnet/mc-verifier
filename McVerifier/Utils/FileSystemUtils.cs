namespace McVerifier.Utils;

public static class FileUtils
{
    public static void CreateDirectory(string consumerContractsDirectory)
    {
        if (Directory.Exists(consumerContractsDirectory))
            Directory.Delete(consumerContractsDirectory, true);

        Directory.CreateDirectory(consumerContractsDirectory);
    }

    public static string GetSolutionDirectory(int depth = 4)
    {
        var path = new List<string> { Directory.GetCurrentDirectory() };
        path.AddRange(GetPathDeep(depth));
        return Path.GetFullPath(Path.Combine(path.ToArray()));
    }

    public static IEnumerable<string> GetPathDeep(int deep)
        => Enumerable.Range(1, deep).Select(c => @"..\");

    /// <param name="pattern">"*{pattern}.json"</param>
    /// <returns></returns>
    public static IReadOnlyCollection<string> GetFilesByPattern(string rootPath, string pattern)
        => Directory.GetFiles(rootPath, pattern, new EnumerationOptions { RecurseSubdirectories = true })
        .ToReadOnly();
}
