using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexTracker.Serialization;

/// <summary>JSON settings for the Tracker API: camelCase names, null fields left out, Tracker's own date-time format.</summary>
internal static class YandexTrackerJson
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new YandexTrackerDateTimeConverter() },
    };
}
