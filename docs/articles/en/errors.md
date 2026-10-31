# Errors and retries

[Русский](../ru/errors.md)

| Response | Exception |
| --- | --- |
| 401 | YandexTrackerUnauthorizedException |
| 403 | YandexTrackerForbiddenException |
| 404 | YandexTrackerNotFoundException |
| 409 | YandexTrackerConflictException |
| 412 | YandexTrackerPreconditionFailedException |
| Other unsuccessful HTTP status | YandexTrackerApiException |
| Invalid response JSON | YandexTrackerSerializationException |

API exceptions expose StatusCode, Error, ErrorMessages and RawResponse. Tracker's own messages are
retained; an unparseable error response still has its raw text. Redact it before logging.
Argument validation, cancellation, exhausted transport failures and timeouts use their standard
.NET exceptions (ArgumentException, OperationCanceledException/TaskCanceledException, HttpRequestException);
they are not wrapped in YandexTrackerException.

~~~csharp
try
{
    var issue = await client.Issues.GetAsync("TEST-1", cancellationToken);
}
catch (YandexTrackerNotFoundException)
{
    Console.WriteLine("Issue is missing or unavailable.");
}
catch (YandexTrackerApiException error)
{
    Console.WriteLine($"{(int)error.StatusCode}: {error.Message}");
}
~~~

MaxRetries defaults to 3 (up to 4 attempts), RetryDelay to 500 milliseconds.
A 429 is retried for every method; a 5xx or HttpRequestException only for reads and search.
Writes are not retried after 5xx/network failures because they might already have succeeded.
Retry-After (delta or date) overrides the exponential delay. Cancellation interrupts waits and requests.
There is no automatic token refresh, circuit breaker or global rate limiter.
