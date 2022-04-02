using System.Reflection;
using System.Text.RegularExpressions;

namespace Core
{
    public static class ReflectionUtils
    {
        public static HashSet<Type> FindTypes(params string[] patterns)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .ToArray();

            return assemblies.SelectMany(assembl => FindByPattern(assembl, patterns)).ToHashSet();
        }

        private static Type[] FindByPattern(Assembly assembly, string[] patterns)
            => assembly.GetTypes()
                      .Where(t => PatternAny(t.FullName, patterns))
                      .ToArray();

        private static bool PatternAny(string fullName, string[] patterns)
            => patterns.Any(pattern => Regex.Match(fullName, pattern, RegexOptions.IgnoreCase).Success);
    }
}
