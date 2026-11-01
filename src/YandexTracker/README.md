# YandexTracker

[English](#english) | [Русский](#русский)

## English

Unofficial asynchronous .NET client for the [Yandex Tracker API](https://yandex.ru/support/tracker/ru/api/about-api)
(v2). Runs on **.NET 8, .NET 9 and .NET 10** and depends on nothing but the BCL.

> Unofficial project, not affiliated with Yandex.

```sh
dotnet add package YandexTracker
```

```csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!,
});

using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var me = await client.MyselfAsync(cancellation.Token);
var issue = await client.Issues.GetAsync("TEST-1", cancellation.Token);
var hours = YandexTrackerDuration.ToHours(issue.OriginalEstimation);   // P1W2D -> 56 working hours

await foreach (var found in client.Issues.FindAsync(new IssueSearch { Query = "Queue: TEST Resolution: empty()" }, cancellation.Token))
    Console.WriteLine($"{found.Key}: {found.Summary}");
```

- OAuth or IAM tokens; Yandex 360 (`X-Org-ID`) or Yandex Cloud (`X-Cloud-Org-ID`) organizations.
- Issues: get, paged search, create (idempotent when reusing `IssueDraft.Unique`), update with version check, links, transitions, comments;
  queue-local fields in `Issue.Fields`.
- Queues, users, statuses, `myself`.
- Retries on 429 (any request) and on 5xx/network failures for reads/search; `Retry-After` is honoured.
- API and serialization exceptions under `YandexTrackerException`; standard argument, transport and cancellation exceptions can propagate.

The SDK does not obtain/refresh tokens or load `.env`. Reuse the client. Passing a caller-owned
`HttpClient` to its constructor leaves that HTTP client alive when the SDK is disposed.

## Русский

Неофициальный асинхронный SDK API Яндекс Трекера для .NET 8, 9 и 10 без runtime NuGet-зависимостей.
Не связан с Яндексом. Установка: `dotnet add package YandexTracker`.

Пример выше использует переменные окружения `YANDEX_TRACKER_TOKEN` и `YANDEX_TRACKER_ORG_ID`, OAuth
и Яндекс 360 по умолчанию. Для IAM/Cloud задайте `TokenType = YandexTrackerTokenType.Iam` и
`OrganizationKind = YandexTrackerOrganizationKind.Cloud`. SDK не получает токены и не загружает `.env`.

Поддерживаются задачи, постраничный поиск, создание с `IssueDraft.Unique`, обновление с проверкой
версии, связи, переходы, комментарии, очереди, пользователи и статусы. `CancellationToken` отменяет
запросы и ожидания. Повторы при 429 доступны всем запросам; после 5xx/сетевой ошибки - только чтению
и поиску. API/ошибки разбора ответа наследуют `YandexTrackerException`; стандартные исключения
аргументов, сети и отмены могут передаваться напрямую.

Переиспользуйте клиент. Переданный извне `HttpClient` остается у вызывающего приложения после
освобождения SDK. Подробные руководства и полный запускаемый пример доступны в репозитории.

Documentation: <https://jrfrigat.github.io/YandexTracker/> · Source: <https://github.com/jrfrigat/YandexTracker>
