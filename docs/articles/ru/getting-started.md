# Начало работы

[English](../en/getting-started.md)

Пакет YandexTracker поддерживает .NET 8, 9 и 10. Выберите опубликованную версию в
[NuGet](https://www.nuget.org/packages/YandexTracker).

~~~sh
dotnet add package YandexTracker
~~~

~~~csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!
});
using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var me = await client.MyselfAsync(cancellation.Token);
Console.WriteLine(me.Display);
await foreach (var issue in client.Issues.FindAsync(
    new IssueSearch { Queue = "TEST", Limit = 20 }, cancellation.Token))
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
~~~

Без аргумента HttpClient SDK создаёт его сам и освобождает при Dispose. Если передать HttpClient первым
аргументом конструктора, его жизненным циклом управляет вызывающее приложение. Переиспользуйте клиент,
а не создавайте новый на каждую задачу. Отдельного DI-пакета нет.
Выбор токена и организации описан в [авторизации](authentication.md).

В репозитории есть `samples/YandexTracker.QuickStart` - полный пример чтения с проверкой настроек,
отменой по Ctrl+C и обработкой ошибок. Из корня выполните
`dotnet run --project samples/YandexTracker.QuickStart -- --help`.

## Совместимость и ограничения

Пакет не имеет runtime NuGet-зависимостей. Модели используют System.Text.Json; неизвестные свойства
задачи сохраняются в Issue.Fields (см. [Задачи и поиск](issues.md)). Совместимость с Native AOT и trimming
не проверялась; сериализация использует reflection.

Вложения, доски, массовые операции, экспорт и получение токенов не реализованы. Поддерживаемые
endpoints перечислены в [README](https://github.com/jrfrigat/YandexTracker/blob/main/README.ru.md#возможности). Это независимый SDK; поведение сервиса
описано в [официальном API-справочнике](https://yandex.ru/support/tracker/ru/api/about-api).
