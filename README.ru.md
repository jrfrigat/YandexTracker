<p align="center">
  <img src="assets/icon.svg" width="112" alt="YandexTracker" />
</p>

<h1 align="center">YandexTracker для .NET</h1>

<p align="center">🌐 <a href="README.md">English</a> · <b>Русский</b></p>

[![CI](https://github.com/jrfrigat/YandexTracker/actions/workflows/ci.yml/badge.svg)](https://github.com/jrfrigat/YandexTracker/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/YandexTracker.svg?logo=nuget)](https://www.nuget.org/packages/YandexTracker)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com)

Неофициальная асинхронная библиотека для [API Яндекс Трекера](https://yandex.ru/support/tracker/ru/api/about-api).
Работает на **.NET 8, .NET 9 и .NET 10**. API вдохновлён частью возможностей официального Python SDK
[yandex_tracker_client](https://github.com/yandex/yandex_tracker_client).

> ⚠️ Неофициальный проект, не связан с Яндексом. Соблюдайте условия использования сервиса.

## Возможности

- ✅ Полностью асинхронный API с `CancellationToken` в каждом вызове; постраничная выдача — через `IAsyncEnumerable`
- ✅ **Задачи** — чтение, поиск по запросу, фильтру, ключам или очереди, создание (идемпотентное через `Unique`),
  правка с проверкой версии, связи, переходы, комментарии; локальные поля очередей — в `Issue.Fields`
- ✅ **Очереди, пользователи, статусы** и `myself`
- ✅ Токены **OAuth или IAM**; организации **Яндекс 360** (`X-Org-ID`) или **Yandex Cloud** (`X-Cloud-Org-ID`)
- ✅ **Повторы** — при 429 повторяется любой запрос, при 5xx и сетевой ошибке — только чтение и поиск;
  учитывается `Retry-After`
- ✅ **Типизированные исключения** с базовым `YandexTrackerException`, словами Трекера и сырым телом ответа
- ✅ **Рабочее время** — `YandexTrackerDuration` переводит `P1W2DT3H` в часы и обратно
- ✅ Без зависимостей кроме BCL; полная XML-документация; Source Link

## Установка

Первая планируемая версия — 0.1.0. До публикации пакета используйте [локальный источник](docs/articles/ru/releasing.md). Сайт и бейджи станут доступны после настройки GitHub и выпуска.

```bash
dotnet add package YandexTracker
```

| Пакет | Назначение |
|-------|-----------|
| [`YandexTracker`](https://www.nuget.org/packages/YandexTracker) | `YandexTrackerClient`, разделы API, модели и исключения. |

## Быстрый старт

```csharp
using YandexTracker;
using YandexTracker.Models.Issues;

// Не храните токен в коде — берите его из окружения или хранилища секретов.
using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!,
    // TokenType = YandexTrackerTokenType.Iam, OrganizationKind = YandexTrackerOrganizationKind.Cloud
});

// Кто я — заодно самая дешёвая проверка подключения
var me = await client.MyselfAsync();

// Задача и её оценка в рабочих часах
var issue = await client.Issues.GetAsync("TEST-1");
var hours = YandexTrackerDuration.ToHours(issue.OriginalEstimation);   // P1W2D → 56

// Поиск: страницы читаются по мере перебора
await foreach (var found in client.Issues.FindAsync(new IssueSearch { Query = "Queue: TEST Resolution: empty()" }))
    Console.WriteLine($"{found.Key}: {found.Summary} [{found.Status?.Key}]");

// Правка с проверкой версии; null очищает поле
await client.Issues.UpdateAsync("TEST-1", new Dictionary<string, object?> { ["summary"] = "Новое название" }, issue.Version);

// Статус меняется переходом
var transitions = await client.Issues.GetTransitionsAsync("TEST-1");
await client.Issues.ExecuteTransitionAsync("TEST-1", transitions.First(t => t.To?.Key == "inProgress").Id);

// Связи, комментарии, очереди, пользователи
var links = await client.Issues.GetLinksAsync("TEST-1");
await client.Issues.AddCommentAsync("TEST-1", "Начал");
await foreach (var queue in client.Queues.GetAllAsync()) Console.WriteLine(queue.Key);
```

## Авторизация

| Организация | Токен | Настройки |
|-------------|-------|-----------|
| Яндекс 360 | OAuth | `OrganizationKind = Yandex360`, `TokenType = OAuth` (по умолчанию) |
| Yandex Cloud | IAM | `OrganizationKind = Cloud`, `TokenType = Iam` |
| Yandex Cloud | OAuth | `OrganizationKind = Cloud`, `TokenType = OAuth` |

Идентификатор организации виден в Трекере: *Администрирование → Организации*. Подробнее:
[Авторизация](docs/articles/ru/authentication.md).

## Ошибки и повторы

```csharp
try
{
    await client.Issues.UpdateAsync("TEST-1", new Dictionary<string, object?> { ["summary"] = "Updated title" }, version: 7);
}
catch (YandexTrackerConflictException)
{
    // задачу кто-то изменил — перечитайте её и повторите
}
catch (YandexTrackerApiException e)
{
    Console.WriteLine($"{(int)e.StatusCode}: {e.Message}");   // слова Трекера; e.RawResponse — для диагностики
}
```

`YandexTrackerException` — база собственных ошибок SDK (также возможны стандартные ошибки аргументов, сети и отмены): отказы API (с подклассами для 401, 403, 404, 409
и 412) и `YandexTrackerSerializationException` для ответа, который не удалось разобрать. Повторы настраиваются через
`MaxRetries` и `RetryDelay`. Подробнее: [Ошибки и повторы](docs/articles/ru/errors.md).

## Документация

Руководства и справочник API: **<https://jrfrigat.github.io/YandexTracker/>**

- [Начало работы](docs/articles/ru/getting-started.md)
- [Авторизация](docs/articles/ru/authentication.md)
- [Задачи и поиск](docs/articles/ru/issues.md)
- [Ошибки и повторы](docs/articles/ru/errors.md)
- [Архитектура](docs/articles/ru/architecture.md)
- [Вопросы и ответы](docs/articles/ru/faq.md)

## Устройство репозитория

```
.
├── src/
│   └── YandexTracker/          # библиотека: клиент, разделы, модели, исключения, HTTP, JSON
├── tests/
│   └── YandexTracker.Tests/    # модульные тесты + интеграционные (только чтение, по токену) (xUnit)
├── docs/                       # сайт документации (DocFX)
├── assets/                     # иконка
└── .github/workflows/          # CI, выпуск в NuGet, CodeQL, публикация документации
```

## Сборка и тесты

```bash
dotnet restore
dotnet build -c Release
dotnet test  -c Release
```

Нужен .NET SDK 10 (он собирает цели net8.0/net9.0/net10.0). Интеграционные тесты читают настоящий API и
**пропускаются автоматически**, пока не заданы `YANDEX_TRACKER_TOKEN` и `YANDEX_TRACKER_ORG_ID` (в окружении или в
`.env`, см. [`.env.example`](.env.example)). Они ничего не меняют в организации.

## Лицензия

[MIT](LICENSE) © FrigaT

Подробнее: [Выпуск и подключение Alva](docs/articles/ru/releasing.md) · [Участие в разработке](CONTRIBUTING.ru.md).
