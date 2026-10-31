using System.Net;

namespace YandexTracker;

/// <summary>
/// Tracker refused a request. The message is Tracker's own <c>errorMessages</c>; the parsed body is in
/// <see cref="Error"/> and the raw one in <see cref="RawResponse"/>. Common status codes get their own subclasses,
/// as in the official Python SDK, so they can be caught without checking the code.
/// </summary>
public class YandexTrackerApiException : YandexTrackerException
{
    /// <summary>Initializes a new instance for a refused request.</summary>
    /// <param name="statusCode">The HTTP status code Tracker answered with.</param>
    /// <param name="error">The parsed error body, if the body looked like one.</param>
    /// <param name="rawResponse">The raw response body, kept for diagnostics.</param>
    public YandexTrackerApiException(HttpStatusCode statusCode, ApiError? error, string? rawResponse)
        : base(BuildMessage(statusCode, error))
    {
        StatusCode = statusCode;
        Error = error;
        RawResponse = rawResponse;
    }

    /// <summary>The HTTP status code Tracker answered with.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>The parsed error body, or <see langword="null"/> when the body did not look like a Tracker error.</summary>
    public ApiError? Error { get; }

    /// <summary>The raw response body, for diagnostics.</summary>
    public string? RawResponse { get; }

    /// <summary>Tracker's error messages without blank entries; empty when it sent none.</summary>
    public IReadOnlyList<string> ErrorMessages => Texts(Error);

    internal static YandexTrackerApiException For(HttpStatusCode status, ApiError? error, string? raw) => status switch
    {
        HttpStatusCode.Unauthorized => new YandexTrackerUnauthorizedException(error, raw),
        HttpStatusCode.Forbidden => new YandexTrackerForbiddenException(error, raw),
        HttpStatusCode.NotFound => new YandexTrackerNotFoundException(error, raw),
        HttpStatusCode.Conflict => new YandexTrackerConflictException(error, raw),
        HttpStatusCode.PreconditionFailed => new YandexTrackerPreconditionFailedException(error, raw),
        _ => new YandexTrackerApiException(status, error, raw),
    };

    // JSON may bring null instead of a list, or blank strings: only real texts go into the message.
    private static string[] Texts(ApiError? error) => (error?.Messages ?? []).Where(m => !string.IsNullOrWhiteSpace(m)).ToArray();

    private static string BuildMessage(HttpStatusCode status, ApiError? error) =>
        Texts(error) is { Length: > 0 } texts ? string.Join(" ", texts)
        : error?.Fields is { Count: > 0 } fields ? string.Join("; ", fields.Select(e => $"{e.Key}: {e.Value}"))
        : $"Yandex Tracker answered {(int)status} {status}.";
}

/// <summary>401: the token was not accepted.</summary>
/// <param name="error">The parsed error body.</param>
/// <param name="rawResponse">The raw response body.</param>
public sealed class YandexTrackerUnauthorizedException(ApiError? error, string? rawResponse)
    : YandexTrackerApiException(HttpStatusCode.Unauthorized, error, rawResponse);

/// <summary>403: the token has no rights for the action or the organization.</summary>
/// <param name="error">The parsed error body.</param>
/// <param name="rawResponse">The raw response body.</param>
public sealed class YandexTrackerForbiddenException(ApiError? error, string? rawResponse)
    : YandexTrackerApiException(HttpStatusCode.Forbidden, error, rawResponse);

/// <summary>404: there is no such issue, queue or user.</summary>
/// <param name="error">The parsed error body.</param>
/// <param name="rawResponse">The raw response body.</param>
public sealed class YandexTrackerNotFoundException(ApiError? error, string? rawResponse)
    : YandexTrackerApiException(HttpStatusCode.NotFound, error, rawResponse);

/// <summary>409: the object has changed - the version in the request is stale.</summary>
/// <param name="error">The parsed error body.</param>
/// <param name="rawResponse">The raw response body.</param>
public sealed class YandexTrackerConflictException(ApiError? error, string? rawResponse)
    : YandexTrackerApiException(HttpStatusCode.Conflict, error, rawResponse);

/// <summary>412: a precondition of the request failed, e.g. the transition is not available.</summary>
/// <param name="error">The parsed error body.</param>
/// <param name="rawResponse">The raw response body.</param>
public sealed class YandexTrackerPreconditionFailedException(ApiError? error, string? rawResponse)
    : YandexTrackerApiException(HttpStatusCode.PreconditionFailed, error, rawResponse);
