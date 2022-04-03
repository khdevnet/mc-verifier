using System.Collections.ObjectModel;

namespace McVerifier.Utils;

public static class CollectionExtensions
{
    public static IReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> items)
        => new ReadOnlyCollection<T>(items.ToList());
}
