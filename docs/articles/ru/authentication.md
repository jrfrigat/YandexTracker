# Авторизация

[English](../en/authentication.md)

SDK требует Token и OrganizationId: без них валидация завершается до первого запроса.
Получение доступов описано в официальном
[руководстве Трекера](https://yandex.ru/support/tracker/ru/concepts/access).

| Настройки | Authorization | Заголовок организации |
| --- | --- | --- |
| OAuth + Yandex360 (по умолчанию) | OAuth token | X-Org-ID |
| OAuth + Cloud | OAuth token | X-Cloud-Org-ID |
| Iam + Cloud | Bearer token | X-Cloud-Org-ID |

~~~csharp
var options = new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!,
    TokenType = YandexTrackerTokenType.Iam,
    OrganizationKind = YandexTrackerOrganizationKind.Cloud,
    AcceptLanguage = "ru"
};
~~~

Приложение передаёт токен; SDK не получает и не обновляет его и не загружает .env.
Приложение загружает свою конфигурацию самостоятельно.
Используйте стандартный HTTPS BaseAddress. Авторизация отправляется на этот адрес,
поэтому не позволяйте недоверенному вводу менять его. Не записывайте токены и приватные ответы в логи.
