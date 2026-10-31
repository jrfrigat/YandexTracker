using YandexTracker.Http;
using YandexTracker.Models.Statuses;

namespace YandexTracker.Endpoints;

/// <summary>The default <see cref="IStatusesClient"/> implementation.</summary>
internal sealed class StatusesClient(IYandexTrackerConnection connection) : IStatusesClient
{
    public async Task<IReadOnlyList<Status>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await connection.GetAsync<List<Status>>("v2/statuses", cancellationToken).ConfigureAwait(false) ?? [];
}
