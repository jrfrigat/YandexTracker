# Authentication

[Русский](../ru/authentication.md)

The SDK requires Token and OrganizationId; missing values fail validation before the first request.
Obtain the token and organization id using the official
[Tracker access guide](https://yandex.ru/support/tracker/ru/concepts/access).

| Options | Authorization | Organization header |
| --- | --- | --- |
| OAuth + Yandex360 (defaults) | OAuth token | X-Org-ID |
| OAuth + Cloud | OAuth token | X-Cloud-Org-ID |
| Iam + Cloud | Bearer token | X-Cloud-Org-ID |

~~~csharp
var options = new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!,
    TokenType = YandexTrackerTokenType.Iam,
    OrganizationKind = YandexTrackerOrganizationKind.Cloud,
    AcceptLanguage = "en"
};
~~~

Tokens are supplied by the application; the SDK neither obtains nor refreshes them.
The SDK does not load .env files; applications must load their own configuration.
Use the default HTTPS BaseAddress. Authorization is attached to requests sent to that address;
do not allow untrusted user input to select it. Do not log tokens or raw private issue data.
