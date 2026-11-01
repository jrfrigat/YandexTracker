using YandexTracker;
using YandexTracker.Models.Issues;

if (args is ["--help"] or ["-h"])
{
    Console.WriteLine("Read the current user and optionally up to 10 issues from a queue.");
    Console.WriteLine("Set YANDEX_TRACKER_TOKEN and YANDEX_TRACKER_ORG_ID.");
    Console.WriteLine("Optional: YANDEX_TRACKER_QUEUE, YANDEX_TRACKER_TOKEN_TYPE=iam|oauth,");
    Console.WriteLine("YANDEX_TRACKER_ORG_KIND=cloud|yandex360. The sample does not load .env.");
    return 0;
}

if (args.Length != 0)
{
    Console.Error.WriteLine("Unknown arguments. Use --help.");
    return 2;
}

using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
ConsoleCancelEventHandler cancelHandler = (_, e) => { e.Cancel = true; cancellation.Cancel(); };
Console.CancelKeyPress += cancelHandler;
try
{
    var options = new YandexTrackerClientOptions
    {
        Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN") ?? "",
        OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID") ?? "",
        TokenType = (Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN_TYPE") ?? "oauth").ToLowerInvariant() switch
        {
            "oauth" => YandexTrackerTokenType.OAuth,
            "iam" => YandexTrackerTokenType.Iam,
            _ => throw new ArgumentException("YANDEX_TRACKER_TOKEN_TYPE must be oauth or iam.")
        },
        OrganizationKind = (Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_KIND") ?? "yandex360").ToLowerInvariant() switch
        {
            "yandex360" => YandexTrackerOrganizationKind.Yandex360,
            "cloud" => YandexTrackerOrganizationKind.Cloud,
            _ => throw new ArgumentException("YANDEX_TRACKER_ORG_KIND must be yandex360 or cloud.")
        }
    };
    using var client = new YandexTrackerClient(options);
    var me = await client.MyselfAsync(cancellation.Token);
    Console.WriteLine($"Current user: {me.Login ?? me.Display}");

    var queue = Environment.GetEnvironmentVariable("YANDEX_TRACKER_QUEUE");
    if (!string.IsNullOrWhiteSpace(queue))
    {
        await foreach (var issue in client.Issues.FindAsync(new IssueSearch { Queue = queue, Limit = 10 }, cancellation.Token))
            Console.WriteLine($"{issue.Key}: {issue.Summary}");
    }
    return 0;
}
catch (OperationCanceledException)
{
    Console.Error.WriteLine("Canceled or the 30-second timeout elapsed.");
    return 1;
}
catch (YandexTrackerApiException error)
{
    Console.Error.WriteLine($"Tracker returned HTTP {(int)error.StatusCode}. Check access and organization settings.");
    return 1;
}
catch (YandexTrackerException)
{
    Console.Error.WriteLine("The Tracker response could not be processed.");
    return 1;
}
catch (HttpRequestException)
{
    Console.Error.WriteLine("The Tracker request failed. Check network access.");
    return 1;
}
catch (ArgumentException error)
{
    Console.Error.WriteLine(error.Message);
    return 2;
}
finally { Console.CancelKeyPress -= cancelHandler; }
