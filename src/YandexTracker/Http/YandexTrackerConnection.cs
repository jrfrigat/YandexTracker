using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using YandexTracker.Serialization;

namespace YandexTracker.Http;

/// <summary>Sends requests to the API: authorization, retries, paging and error parsing. Shared by every endpoint group.</summary>
internal interface IYandexTrackerConnection
{
    Task<T> GetAsync<T>(string path, CancellationToken ct);

    Task<T> SendAsync<T>(HttpMethod method, string path, object? body, bool safe, CancellationToken ct);

    IAsyncEnumerable<T> PagesAsync<T>(HttpMethod method, string path, object? body, bool safe, int pageSize, int? limit, CancellationToken ct);
}

/// <inheritdoc cref="IYandexTrackerConnection"/>
internal sealed class YandexTrackerConnection(HttpClient http, YandexTrackerClientOptions options, bool ownsHttp) : IYandexTrackerConnection, IDisposable
{
    public Task<T> GetAsync<T>(string path, CancellationToken ct) => SendAsync<T>(HttpMethod.Get, path, null, safe: true, ct);

    public async Task<T> SendAsync<T>(HttpMethod method, string path, object? body, bool safe, CancellationToken ct)
    {
        using var response = await SendRawAsync(method, path, body, safe, ct).ConfigureAwait(false);
        return await Read<T>(response, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Paged results: <c>perPage</c> and <c>page</c> in the address, the end found by <c>X-Total-Pages</c> or by a short
    /// page. Items are yielded as they are read, so a large search never holds everything in memory.
    /// </summary>
    public async IAsyncEnumerable<T> PagesAsync<T>(HttpMethod method, string path, object? body, bool safe, int pageSize, int? limit,
        [EnumeratorCancellation] CancellationToken ct)
    {
        if (pageSize is < 1 or > 100) throw new ArgumentOutOfRangeException(nameof(pageSize), "The page size must be between 1 and 100.");
        var given = 0;
        for (var page = 1; ; page++)
        {
            var separator = path.Contains('?', StringComparison.Ordinal) ? '&' : '?';
            using var response = await SendRawAsync(method, $"{path}{separator}perPage={pageSize}&page={page}", body, safe, ct).ConfigureAwait(false);
            var items = await Read<List<T>>(response, ct).ConfigureAwait(false) ?? [];
            foreach (var item in items)
            {
                if (limit is { } max && given >= max) yield break;
                yield return item;
                given++;
            }
            var totalPages = response.Headers.TryGetValues("X-Total-Pages", out var values)
                && int.TryParse(values.FirstOrDefault(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var total) ? total : (int?)null;
            if (items.Count < pageSize || totalPages is { } last && page >= last || limit is { } cap && given >= cap) yield break;
        }
    }

    public void Dispose()
    {
        if (ownsHttp) http.Dispose();
    }

    private async Task<HttpResponseMessage> SendRawAsync(HttpMethod method, string path, object? body, bool safe, CancellationToken ct)
    {
        var delay = options.RetryDelay;
        for (var attempt = 0; ; attempt++)
        {
            // A fresh message per attempt: an HttpRequestMessage cannot be sent twice.
            using var request = Build(method, path, body);
            HttpResponseMessage response;
            try
            {
                response = await http.SendAsync(request, ct).ConfigureAwait(false);
            }
            catch (HttpRequestException) when (safe && attempt < options.MaxRetries)
            {
                await Task.Delay(delay, ct).ConfigureAwait(false);
                delay *= 2;
                continue;
            }
            if (response.IsSuccessStatusCode) return response;
            // A 429 means the request was not processed, so any request may be repeated. A 5xx may have changed data
            // already, so only safe requests - reads and searches - are repeated after one.
            var retry = response.StatusCode == HttpStatusCode.TooManyRequests || safe && (int)response.StatusCode >= 500;
            if (retry && attempt < options.MaxRetries)
            {
                var wait = response.Headers.RetryAfter?.Delta
                    ?? (response.Headers.RetryAfter?.Date is { } at ? at - DateTimeOffset.UtcNow : (TimeSpan?)null)
                    ?? delay;
                response.Dispose();
                await Task.Delay(wait < TimeSpan.Zero ? TimeSpan.Zero : wait, ct).ConfigureAwait(false);
                delay *= 2;
                continue;
            }
            using (response) throw await Error(response, ct).ConfigureAwait(false);
        }
    }

    private HttpRequestMessage Build(HttpMethod method, string path, object? body)
    {
        var request = new HttpRequestMessage(method, new Uri(options.BaseAddress, path));
        try
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(options.TokenType == YandexTrackerTokenType.Iam ? "Bearer" : "OAuth", options.Token);
            request.Headers.Add(options.OrganizationKind == YandexTrackerOrganizationKind.Cloud ? YandexTrackerHeaders.CloudOrganization : YandexTrackerHeaders.Organization,
                options.OrganizationId);
            if (options.AcceptLanguage is { Length: > 0 } language) request.Headers.AcceptLanguage.ParseAdd(language);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (body is not null) request.Content = JsonContent.Create(body, body.GetType(), options: YandexTrackerJson.Options);
            return request;
        }
        catch
        {
            request.Dispose();
            throw;
        }
    }

    /// <summary>Reads the body as a model; an unreadable answer is a <see cref="YandexTrackerSerializationException"/>, not a bare JsonException.</summary>
    private static async Task<T> Read<T>(HttpResponseMessage response, CancellationToken ct)
    {
        if (response.StatusCode == HttpStatusCode.NoContent || response.Content.Headers.ContentLength == 0) return default!;
        try
        {
            return (await response.Content.ReadFromJsonAsync<T>(YandexTrackerJson.Options, ct).ConfigureAwait(false))!;
        }
        catch (JsonException e)
        {
            throw new YandexTrackerSerializationException($"The Yandex Tracker response could not be read as {typeof(T).Name}.", e);
        }
    }

    private static async Task<YandexTrackerApiException> Error(HttpResponseMessage response, CancellationToken ct)
    {
        string? text = null;
        ApiError? error = null;
        try
        {
            text = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            if (text.Length > 0) error = JsonSerializer.Deserialize<ApiError>(text, YandexTrackerJson.Options);
        }
        catch (JsonException)
        {
            // Not a Tracker error body (an HTML page from a proxy, say): the raw text still travels with the exception.
        }
        return YandexTrackerApiException.For(response.StatusCode, error, text);
    }
}
