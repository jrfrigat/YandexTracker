using YandexTracker.Endpoints;
using YandexTracker.Models.Users;

namespace YandexTracker;

/// <summary>
/// A client of the Yandex Tracker API (v2). Its endpoint groups - <see cref="Issues"/>, <see cref="Queues"/>,
/// <see cref="Users"/> and <see cref="Statuses"/> - mirror the attributes of <c>TrackerClient</c> in the official
/// Python SDK.
/// </summary>
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
