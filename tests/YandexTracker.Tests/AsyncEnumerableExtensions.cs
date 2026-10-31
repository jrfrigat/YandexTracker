namespace YandexTracker.Tests;

#if !NET10_0_OR_GREATER
/// <summary>
/// <c>ToListAsync</c> for <see cref="IAsyncEnumerable{T}"/> on the targets that predate
/// System.Linq.AsyncEnumerable (it ships in the BCL from .NET 10).
/// </summary>
internal static class AsyncEnumerableExtensions
{
    public static async ValueTask<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        var list = new List<T>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false)) list.Add(item);
        return list;
    }
}
#endif
