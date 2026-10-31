# Задачи и поиск

[English](../en/issues.md)

Задайте ровно одно условие IssueSearch: Query, Filter, Keys либо Queue. Order работает только с Filter.
PageSize — от 1 до 100; Limit ограничивает общее число результатов. Страницы запрашиваются при
перечислении. Это обычная пагинация, а не снимок данных или export/scroll API: изменения задач во время
обхода могут повлиять на результат.

~~~csharp
using YandexTracker.Models.Issues;

var search = new IssueSearch
{
    Filter = new Dictionary<string, object?> { ["queue"] = "TEST", ["status"] = "open" },
    Order = "+updatedAt",
    PageSize = 50,
    Limit = 200
};
await foreach (var issue in client.Issues.FindAsync(search, cancellationToken))
{
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
    if (issue.Fields?.TryGetValue("customField", out var value) == true)
        Console.WriteLine(value);
}
~~~

~~~csharp
var created = await client.Issues.CreateAsync(new IssueDraft
{
    Queue = "TEST",
    Summary = "Проверить импорт",
    Unique = Guid.NewGuid().ToString(),
    OriginalEstimation = YandexTrackerDuration.FromHours(10)
});
var updated = await client.Issues.UpdateAsync(created.Key,
    new Dictionary<string, object?> { ["summary"] = "Проверить импорт SDK" }, created.Version);
var transitions = await client.Issues.GetTransitionsAsync(updated.Key);
var transition = transitions.FirstOrDefault(t => t.To?.Key == "inProgress");
if (transition is not null)
    await client.Issues.ExecuteTransitionAsync(updated.Key, transition.Id, "Начато");
await client.Issues.AddCommentAsync(updated.Key, "Интеграция SDK проверена");
var links = await client.Issues.GetLinksAsync(updated.Key);
~~~

При явном повторе создания сохраняйте Unique исходного запроса. Передавайте Version для защиты от
перезаписи чужих правок. Статус меняется доступным переходом, а не прямой записью поля.
Issue.Fields сохраняет неизвестные JSON-поля; читайте JsonElement с учётом схемы очереди.

Даты без времени представлены DateOnly, временные метки — DateTimeOffset. Конвертация длительностей
считает день равным 8 часам, неделю — 5 дням. Это преобразование, а не производственный календарь
или правило планирования Alva.
