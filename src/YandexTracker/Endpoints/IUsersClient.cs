using YandexTracker.Models.Users;

namespace YandexTracker.Endpoints;

/// <summary>The organization's users.</summary>
public interface IUsersClient
{
    /// <summary>Gets a user by uid or login.</summary>
    /// <param name="idOrLogin">The user's uid or login.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The user.</returns>
    Task<User> GetAsync(string idOrLogin, CancellationToken cancellationToken = default);

    /// <summary>Lists every user of the organization, page by page.</summary>
    /// <param name="cancellationToken">A token to cancel the listing.</param>
    /// <returns>The users.</returns>
    IAsyncEnumerable<User> GetAllAsync(CancellationToken cancellationToken = default);
}
