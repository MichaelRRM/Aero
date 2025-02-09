namespace Aero.Base.Extensions;

public static class EnumerableExtensions
{
    public static Dictionary<TKey, List<TValue>> ToDictionaryList<TSource, TKey, TValue>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TSource, TValue> valueSelector) where TKey : notnull
    {
        var dictionary = new Dictionary<TKey, List<TValue>>();

        foreach (var item in source)
        {
            var key = keySelector(item);
            var value = valueSelector(item);

            if (!dictionary.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                dictionary[key] = list;
            }

            list.Add(value);
        }

        return dictionary;
    }

    public static Dictionary<TKey, List<TSource>> ToDictionaryList<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector) where TKey : notnull
    {
        return source.ToDictionaryList(keySelector, x => x);
    }
}