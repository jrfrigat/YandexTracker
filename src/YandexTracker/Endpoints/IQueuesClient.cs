using YandexTracker.Models.Queues;

namespace YandexTracker.Endpoints;

/// <summary>The organization's queues.</summary>
public interface IQueuesClient
{
    /// <summary>Gets a queue by its key.</summary>
    /// <param name="key">The queue key, e.g. <c>TEST</c>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The queue.</returns>
    Task<Queue> GetAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Lists every queue of the organization, page by page.</summary>
    /// <param name="cancellationToken">A token to cancel the listing.</param>
    /// <returns>The queues.</returns>
    IAsyncEnumerable<Queue> GetAllAsync(CancellationToken cancellationToken = default);
}
