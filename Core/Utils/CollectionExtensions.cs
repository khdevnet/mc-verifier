using System.Collections.ObjectModel;

namespace Core
{
    public static class CollectionExtensions
    {
        public static IReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> items)
            => new ReadOnlyCollection<T>(items.ToList());
    }
}
