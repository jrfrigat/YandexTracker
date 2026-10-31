using YandexTracker.Models.Issues;

namespace YandexTracker.Endpoints;

/// <summary>Issues: get, search, create, update, links, transitions and comments.</summary>
public interface IIssuesClient
{
    /// <summary>Gets an issue by its key (<c>QUEUE-1</c>) or id.</summary>
    /// <param name="key">The issue key or id.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The issue.</returns>
    /// <exception cref="YandexTrackerNotFoundException">There is no such issue.</exception>
    Task<Issue> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Searches issues. Results are paged; issues arrive as the pages are read.</summary>
    /// <param name="search">The search conditions.</param>
    /// <param name="cancellationToken">A token to cancel the search.</param>
    /// <returns>The matching issues.</returns>
    IAsyncEnumerable<Issue> FindAsync(IssueSearch search, CancellationToken cancellationToken = default);

    /// <summary>Creates an issue. With <see cref="IssueDraft.Unique"/> a repeated request does not create a second one.</summary>
    /// <param name="draft">The new issue.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created issue.</returns>
    Task<Issue> CreateAsync(IssueDraft draft, CancellationToken cancellationToken = default);

    /// <summary>Changes issue fields.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="fields">Field → new value; <see langword="null"/> clears the field.</param>
    /// <param name="version">The version the change is based on; a stale one is rejected with <see cref="YandexTrackerConflictException"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The updated issue.</returns>
    Task<Issue> UpdateAsync(string key, IReadOnlyDictionary<string, object?> fields, long? version = null, CancellationToken cancellationToken = default);

    /// <summary>Gets the issue's links.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The links, as seen from this issue.</returns>
    Task<IReadOnlyList<IssueLink>> GetLinksAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Links the issue to another one.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="relationship">One of <see cref="LinkRelationships"/>.</param>
    /// <param name="otherKey">The other issue's key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created link.</returns>
    Task<IssueLink> LinkAsync(string key, string relationship, string otherKey, CancellationToken cancellationToken = default);

    /// <summary>Gets the transitions currently available to the issue.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The available transitions.</returns>
    Task<IReadOnlyList<Transition>> GetTransitionsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Executes a transition: the only way to change an issue's status.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="transitionId">The transition id from <see cref="GetTransitionsAsync"/>.</param>
    /// <param name="comment">A comment to leave with the transition.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The transitions available after the change.</returns>
    Task<IReadOnlyList<Transition>> ExecuteTransitionAsync(string key, string transitionId, string? comment = null, CancellationToken cancellationToken = default);

    /// <summary>Gets the issue's comments.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The comments.</returns>
    Task<IReadOnlyList<Comment>> GetCommentsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Adds a comment to the issue.</summary>
    /// <param name="key">The issue key.</param>
    /// <param name="text">The comment text.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The created comment.</returns>
    Task<Comment> AddCommentAsync(string key, string text, CancellationToken cancellationToken = default);
}
