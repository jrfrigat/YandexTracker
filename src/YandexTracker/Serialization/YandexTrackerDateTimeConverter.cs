using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexTracker.Serialization;

/// <summary>
/// Tracker writes times as <c>2017-06-11T05:16:01.339+0000</c> - an offset without a colon, which System.Text.Json
/// does not accept. This converter reads that form as well as plain ISO 8601, and writes the form Tracker expects.
/// </summary>
internal sealed class YandexTrackerDateTimeConverter : JsonConverter<DateTimeOffset>
{
    private static readonly string[] Formats = ["yyyy-MM-dd'T'HH:mm:ss.fffzzz", "yyyy-MM-dd'T'HH:mm:ss.fffK", "yyyy-MM-dd'T'HH:mm:ssK", "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK"];

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString() ?? throw new JsonException("An empty date.");
        // +0000 → +00:00, after which the standard parser understands it.
        if (text.Length > 5 && (text[^5] is '+' or '-') && char.IsDigit(text[^4]) && text[^3] != ':')
            text = string.Concat(text.AsSpan(0, text.Length - 2), ":", text.AsSpan(text.Length - 2));
        if (DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var value)) return value;
        if (DateTimeOffset.TryParseExact(text, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)) return value;
        throw new JsonException($"Could not read the Tracker date '{text}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'+0000'", CultureInfo.InvariantCulture));
}
