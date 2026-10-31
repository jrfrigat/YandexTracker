using YandexTracker.Http;
using YandexTracker.Models.Queues;

namespace YandexTracker.Endpoints;

/// <summary>The default <see cref="IQueuesClient"/> implementation.</summary>
internal sealed class QueuesClient(IYandexTrackerConnection connection) : IQueuesClient
{
    public Task<Queue> GetAsync(string key, CancellationToken cancellationToken = default) =>
        connection.GetAsync<Queue>($"v2/queues/{Uri.EscapeDataString(key)}", cancellationToken);

    public IAsyncEnumerable<Queue> GetAllAsync(CancellationToken cancellationToken = default) =>
        connection.PagesAsync<Queue>(HttpMethod.Get, "v2/queues/", null, safe: true, 100, null, cancellationToken);
}
