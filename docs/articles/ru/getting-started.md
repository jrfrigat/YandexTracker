# Начало работы

[English](../en/getting-started.md)

Пакет YandexTracker поддерживает .NET 8, 9 и 10. Первая планируемая версия — 0.1.0.
До её публикации используйте [локальную сборку пакета](releasing.md).

~~~sh
dotnet add package YandexTracker --version 0.1.0
~~~

~~~csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!
});
var me = await client.MyselfAsync();
Console.WriteLine(me.Display);
using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
await foreach (var issue in client.Issues.FindAsync(
    new IssueSearch { Queue = "TEST", Limit = 20 }, cancellation.Token))
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
~~~

Без аргумента HttpClient SDK создаёт его сам и освобождает при Dispose. Если передать HttpClient первым
аргументом конструктора, его жизненным циклом управляет вызывающее приложение. Переиспользуйте клиент,
а не создавайте новый на каждую задачу. Отдельного DI-пакета нет.
Выбор токена и организации описан в [авторизации](authentication.md).
