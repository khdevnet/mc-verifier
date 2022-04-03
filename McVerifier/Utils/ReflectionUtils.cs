using System.Reflection;
using System.Text.RegularExpressions;

namespace McVerifier.Utils;

public static class ReflectionUtils
{
    public static HashSet<Type> FindTypes(params string[] patterns)
        => AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(assembl => FindByPattern(assembl, patterns))
            .ToHashSet();

    private static Type[] FindByPattern(Assembly assembly, string[] patterns)
        => assembly.GetTypes()
                  .Where(t => PatternAny(t.FullName, patterns))
                  .ToArray();

    private static bool PatternAny(string fullName, string[] patterns)
        => patterns.Any(pattern => Regex.Match(fullName, pattern, RegexOptions.IgnoreCase).Success);
}
