using Xunit;

namespace YandexTracker.Tests.Integration;

/// <summary>
/// A <see cref="FactAttribute"/> that runs only when a Tracker token and organization are configured - through the
/// environment or a <c>.env</c> file at the repository root; otherwise the test is skipped. Used for tests that hit
/// the live Yandex Tracker API.
/// </summary>
public sealed class IntegrationFactAttribute : FactAttribute
{
    /// <summary>The variable that supplies the OAuth token.</summary>
    public const string TokenVariable = "YANDEX_TRACKER_TOKEN";

    /// <summary>The variable that supplies the organization id.</summary>
    public const string OrganizationVariable = "YANDEX_TRACKER_ORG_ID";

    /// <summary>Optional: <c>cloud</c> for a Yandex Cloud organization; Yandex 360 otherwise.</summary>
    public const string OrganizationKindVariable = "YANDEX_TRACKER_ORG_KIND";

    /// <summary>Optional: a queue key the search test reads from.</summary>
    public const string QueueVariable = "YANDEX_TRACKER_QUEUE";

    public IntegrationFactAttribute()
    {
        if (Setting(TokenVariable) is null || Setting(OrganizationVariable) is null)
            Skip = $"Set {TokenVariable} and {OrganizationVariable} (environment or .env) to run tests against the live Yandex Tracker API.";
    }

    /// <summary>A setting from the environment, falling back to the repository's <c>.env</c> file.</summary>
    public static string? Setting(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
        return DotEnv.Value.TryGetValue(name, out var fromFile) && !string.IsNullOrWhiteSpace(fromFile) ? fromFile.Trim() : null;
    }

    private static readonly Lazy<Dictionary<string, string>> DotEnv = new(() =>
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var file = Path.Combine(dir.FullName, ".env");
            if (!File.Exists(file)) continue;
            foreach (var line in File.ReadAllLines(file))
            {
                var trimmed = line.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;
                var at = trimmed.IndexOf('=', StringComparison.Ordinal);
                if (at > 0) values[trimmed[..at].Trim()] = trimmed[(at + 1)..].Trim().Trim('"');
            }
            break;
        }
        return values;
    });
}
