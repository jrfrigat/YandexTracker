using System.Globalization;
using System.Text.RegularExpressions;

namespace YandexTracker;

/// <summary>
/// Tracker durations (original estimate, remaining estimate, time spent) are ISO 8601 strings such as
/// <c>P1W2DT3H30M</c>, counted in working time: a week is 5 working days and a day is 8 hours. This helper converts
/// them to hours and back by the same rules.
/// </summary>
public static partial class YandexTrackerDuration
{
    [GeneratedRegex(@"^P(?:(?<w>\d+(?:[.,]\d+)?)W)?(?:(?<d>\d+(?:[.,]\d+)?)D)?(?:T(?:(?<h>\d+(?:[.,]\d+)?)H)?(?:(?<m>\d+(?:[.,]\d+)?)M)?(?:(?<s>\d+(?:[.,]\d+)?)S)?)?$", RegexOptions.CultureInvariant)]
    private static partial Regex Pattern();

    /// <summary>Converts a Tracker duration to working hours.</summary>
    /// <param name="value">A duration such as <c>P1DT4H</c>.</param>
    /// <param name="hoursPerDay">Working hours in a day; 8 by default.</param>
    /// <param name="daysPerWeek">Working days in a week; 5 by default.</param>
    /// <returns>The hours, or <see langword="null"/> for an empty or unrecognized value.</returns>
    public static decimal? ToHours(string? value, decimal hoursPerDay = 8m, decimal daysPerWeek = 5m)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var text = value.Trim().ToUpperInvariant();
        var match = Pattern().Match(text);
        if (!match.Success || text.Length == 1) return null;
        decimal Part(string name) => match.Groups[name].Success
            ? decimal.Parse(match.Groups[name].Value.Replace(',', '.'), CultureInfo.InvariantCulture) : 0m;
        return Part("w") * daysPerWeek * hoursPerDay + Part("d") * hoursPerDay + Part("h") + Part("m") / 60m + Part("s") / 3600m;
    }

    /// <summary>Converts working hours to a Tracker duration: whole days as <c>D</c>, the rest as hours and minutes.</summary>
    /// <param name="hours">The hours; must not be negative.</param>
    /// <param name="hoursPerDay">Working hours in a day; 8 by default.</param>
    /// <returns>A duration such as <c>P1DT4H30M</c>; zero is <c>PT0H</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="hours"/> is negative.</exception>
    public static string FromHours(decimal hours, decimal hoursPerDay = 8m)
    {
        if (hours < 0) throw new ArgumentOutOfRangeException(nameof(hours), "A duration cannot be negative.");
        var days = (int)Math.Floor(hours / hoursPerDay);
        var rest = hours - days * hoursPerDay;
        var wholeHours = (int)Math.Floor(rest);
        var minutes = (int)Math.Round((rest - wholeHours) * 60m);
        if (minutes == 60) { wholeHours++; minutes = 0; }
        var time = (wholeHours > 0 ? $"{wholeHours}H" : "") + (minutes > 0 ? $"{minutes}M" : "");
        var result = "P" + (days > 0 ? $"{days}D" : "") + (time.Length > 0 ? "T" + time : "");
        return result == "P" ? "PT0H" : result;
    }
}
