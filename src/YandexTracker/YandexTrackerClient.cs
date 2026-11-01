using YandexTracker.Endpoints;
using YandexTracker.Http;
using YandexTracker.Models.Users;

namespace YandexTracker;

/// <summary>
/// The default <see cref="IYandexTrackerClient"/>. Pass an <see cref="HttpClient"/> to let the application's factory
/// manage it; without one the client creates its own and disposes of it.
/// </summary>
public sealed class YandexTrackerClient : IYandexTrackerClient
{
    private readonly YandexTrackerConnection _connection;

    /// <summary>Creates a client with its own <see cref="HttpClient"/>.</summary>
    /// <param name="options">Connection settings; validated with <see cref="YandexTrackerClientOptions.Validate"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException">The token or organization id is missing.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The retry count is negative.</exception>
    public YandexTrackerClient(YandexTrackerClientOptions options) : this(new HttpClient(), options, ownsHttp: true) { }

    /// <summary>Creates a client over an <see cref="HttpClient"/> owned by the caller.</summary>
    /// <param name="http">The HTTP client to send requests with; it is not disposed with this client.</param>
    /// <param name="options">Connection settings; validated with <see cref="YandexTrackerClientOptions.Validate"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="http"/> or <paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException">The token or organization id is missing.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The retry count is negative.</exception>
    public YandexTrackerClient(HttpClient http, YandexTrackerClientOptions options) : this(http, options, ownsHttp: false) { }

    private YandexTrackerClient(HttpClient http, YandexTrackerClientOptions options, bool ownsHttp)
    {
        ArgumentNullException.ThrowIfNull(http);
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();
        _connection = new YandexTrackerConnection(http, options, ownsHttp);
        Issues = new IssuesClient(_connection);
        Queues = new QueuesClient(_connection);
        Users = new UsersClient(_connection);
        Statuses = new StatusesClient(_connection);
    }

    /// <inheritdoc />
    public IIssuesClient Issues { get; }

    /// <inheritdoc />
    public IQueuesClient Queues { get; }

    /// <inheritdoc />
    public IUsersClient Users { get; }

    /// <inheritdoc />
    public IStatusesClient Statuses { get; }

    /// <inheritdoc />
    public Task<User> MyselfAsync(CancellationToken cancellationToken = default) =>
        _connection.GetAsync<User>("v2/myself", cancellationToken);

    /// <summary>Disposes of the <see cref="HttpClient"/> when the client created it itself.</summary>
    public void Dispose() => _connection.Dispose();
}
