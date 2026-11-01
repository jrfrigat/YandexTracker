using YandexTracker.Endpoints;
using YandexTracker.Models.Users;

namespace YandexTracker;

/// <summary>
/// A client of the Yandex Tracker API (v2), with endpoint groups for <see cref="Issues"/>,
/// <see cref="Queues"/>, <see cref="Users"/> and <see cref="Statuses"/>.
/// </summary>
/// <remarks>
/// API failures use <see cref="YandexTrackerApiException"/>; invalid responses use
/// <see cref="YandexTrackerSerializationException"/>. Standard argument, transport and cancellation
/// exceptions can also propagate. Pass a cancellation token to bound each operation.
/// </remarks>
public interface IYandexTrackerClient : IDisposable
{
    /// <summary>Issues: get, search, create, update, links, transitions and comments.</summary>
    IIssuesClient Issues { get; }

    /// <summary>The organization's queues.</summary>
    IQueuesClient Queues { get; }

    /// <summary>The organization's users.</summary>
    IUsersClient Users { get; }

    /// <summary>The organization's issue statuses.</summary>
    IStatusesClient Statuses { get; }

    /// <summary>The user the token acts as; a cheap way to check the connection.</summary>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The current user.</returns>
    Task<User> MyselfAsync(CancellationToken cancellationToken = default);
}
