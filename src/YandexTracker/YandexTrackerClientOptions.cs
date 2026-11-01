using YandexTracker.Http;

namespace YandexTracker;

/// <summary>The kind of access token: a Yandex OAuth token or a Yandex Cloud IAM token.</summary>
public enum YandexTrackerTokenType
{
    /// <summary>An OAuth token, sent as <c>Authorization: OAuth &lt;token&gt;</c>.</summary>
    OAuth,

    /// <summary>An IAM token, sent as <c>Authorization: Bearer &lt;token&gt;</c>.</summary>
    Iam,
}

/// <summary>Where the Tracker organization lives; it decides which header carries the organization id.</summary>
public enum YandexTrackerOrganizationKind
{
    /// <summary>A Yandex 360 organization, identified by the <c>X-Org-ID</c> header.</summary>
    Yandex360,

    /// <summary>A Yandex Cloud organization, identified by the <c>X-Cloud-Org-ID</c> header.</summary>
    Cloud,
}

/// <summary>Connection settings for <see cref="YandexTrackerClient"/>.</summary>
public sealed class YandexTrackerClientOptions
{
    /// <summary>The default API address, <c>https://api.tracker.yandex.net/</c>.</summary>
    public static readonly Uri DefaultBaseAddress = new(YandexTrackerHosts.Api);

    /// <summary>The API address; request paths such as <c>v2/issues</c> are resolved against it.</summary>
    public Uri BaseAddress { get; set; } = DefaultBaseAddress;

    /// <summary>The access token.</summary>
    public string Token { get; set; } = "";

    /// <summary>Whether <see cref="Token"/> is an OAuth or an IAM token. Defaults to <see cref="YandexTrackerTokenType.OAuth"/>.</summary>
    public YandexTrackerTokenType TokenType { get; set; } = YandexTrackerTokenType.OAuth;

    /// <summary>The organization id the token works in.</summary>
    public string OrganizationId { get; set; } = "";

    /// <summary>Whether <see cref="OrganizationId"/> is a Yandex 360 or a Yandex Cloud organization.</summary>
    public YandexTrackerOrganizationKind OrganizationKind { get; set; } = YandexTrackerOrganizationKind.Yandex360;

    /// <summary>
    /// Maximum retries after a 429 for any request, or after a 5xx or network error for reads and searches.
    /// Defaults to 3; zero disables retries. Cancellation and authentication failures are not retried.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>The first pause before a retry when the server names none; it doubles on every next attempt.</summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromMilliseconds(500);

    /// <summary>The language of texts in responses (status names, errors); <see langword="null"/> keeps the organization's default.</summary>
    public string? AcceptLanguage { get; set; }

    /// <summary>
    /// Checks that the connection is fully described. Without a token or an organization the API would answer with an
    /// opaque 401, so the client refuses to start instead.
    /// </summary>
    /// <exception cref="ArgumentException">The token or the organization id is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="MaxRetries"/> is negative.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Token)) throw new ArgumentException("A Yandex Tracker access token is required.", nameof(Token));
        if (string.IsNullOrWhiteSpace(OrganizationId)) throw new ArgumentException("A Yandex Tracker organization id is required.", nameof(OrganizationId));
        if (MaxRetries < 0) throw new ArgumentOutOfRangeException(nameof(MaxRetries), "The retry count cannot be negative.");
    }
}
