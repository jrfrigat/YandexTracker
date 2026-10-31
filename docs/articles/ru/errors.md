# Ошибки и повторы

[English](../en/errors.md)

| Ответ | Исключение |
| --- | --- |
| 401 | YandexTrackerUnauthorizedException |
| 403 | YandexTrackerForbiddenException |
| 404 | YandexTrackerNotFoundException |
| 409 | YandexTrackerConflictException |
| 412 | YandexTrackerPreconditionFailedException |
| Остальной неуспешный HTTP-код | YandexTrackerApiException |
| Неверный JSON ответа | YandexTrackerSerializationException |

Исключения API содержат StatusCode, Error, ErrorMessages и RawResponse. Слова Трекера сохраняются;
если тело ошибки не разобрано, доступен исходный текст. Обезличивайте его перед логированием.
Проверка аргументов, отмена, таймаут и исчерпанные сетевые ошибки используют стандартные исключения
.NET: ArgumentException, OperationCanceledException/TaskCanceledException, HttpRequestException.
Они не оборачиваются в YandexTrackerException.

~~~csharp
try
{
    var issue = await client.Issues.GetAsync("TEST-1", cancellationToken);
}
catch (YandexTrackerNotFoundException)
{
    Console.WriteLine("Задача отсутствует или недоступна.");
}
catch (YandexTrackerApiException error)
{
    Console.WriteLine($"{(int)error.StatusCode}: {error.Message}");
}
~~~

MaxRetries по умолчанию равен 3 (до 4 попыток), RetryDelay - 500 мс.
429 повторяется для любого метода; 5xx и HttpRequestException - только для чтения и поиска.
Изменения не повторяются после сетевой ошибки/5xx: они могли уже примениться.
Retry-After в секундах или датой заменяет экспоненциальную задержку. Отмена прерывает запросы и ожидание.
Автообновления токена, circuit breaker и общего ограничителя частоты нет.
