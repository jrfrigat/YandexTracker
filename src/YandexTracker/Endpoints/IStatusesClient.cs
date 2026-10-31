using YandexTracker.Models.Statuses;

namespace YandexTracker.Endpoints;

/// <summary>The organization's issue statuses.</summary>
public interface IStatusesClient
{
    /// <summary>Lists every issue status of the organization.</summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The statuses.</returns>
    Task<IReadOnlyList<Status>> GetAllAsync(CancellationToken cancellationToken = default);
}
