using System.Text.Json.Serialization;

namespace YandexTracker;

/// <summary>The error body of a Tracker response: general messages and per-field errors.</summary>
public sealed record ApiError
{
    /// <summary>General error messages (<c>errorMessages</c>).</summary>
    [JsonPropertyName("errorMessages")] public IReadOnlyList<string>? Messages { get; init; }

    /// <summary>Errors by field name (<c>errors</c>).</summary>
    [JsonPropertyName("errors")] public IReadOnlyDictionary<string, string>? Fields { get; init; }
}
