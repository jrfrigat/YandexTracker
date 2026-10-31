namespace YandexTracker.Models.Issues;

/// <summary>
/// Issue search conditions. Set exactly one of <see cref="Query"/> (Tracker's query language), <see cref="Filter"/>
/// (field → value), <see cref="Keys"/> or <see cref="Queue"/>. <see cref="Order"/> works only with a filter.
/// </summary>
public sealed record IssueSearch
{
    /// <summary>A query in Tracker's query language, e.g. <c>Queue: TEST Resolution: empty()</c>.</summary>
    public string? Query { get; init; }

    /// <summary>Field → value pairs that issues must match.</summary>
    public IReadOnlyDictionary<string, object?>? Filter { get; init; }

    /// <summary>Exact issue keys to return.</summary>
    public IReadOnlyList<string>? Keys { get; init; }

    /// <summary>A queue key: every issue in the queue.</summary>
    public string? Queue { get; init; }

    /// <summary>The sort order for <see cref="Filter"/>, e.g. <c>+updatedAt</c> or <c>-updatedAt</c>.</summary>
    public string? Order { get; init; }

    /// <summary>Issues per page, from 1 to 100. Defaults to 100.</summary>
    public int PageSize { get; init; } = 100;

    /// <summary>How many issues to return in total; <see langword="null"/> returns them all.</summary>
    public int? Limit { get; init; }

    internal object Body()
    {
        var given = new[] { Query is not null, Filter is not null, Keys is not null, Queue is not null }.Count(x => x);
        if (given != 1) throw new ArgumentException("Set exactly one search condition: Query, Filter, Keys or Queue.");
        if (Order is not null && Filter is null) throw new ArgumentException("Order works only with Filter.");
        if (Query is not null) return new { query = Query };
        if (Keys is not null) return new { keys = Keys };
        if (Queue is not null) return new { queue = Queue };
        return Order is null ? new { filter = Filter } : new { filter = Filter, order = Order };
    }
}
