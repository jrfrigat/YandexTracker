# Пример чтения данных

[English](README.md)

Для сборки нужен SDK из `global.json`, для запуска - .NET 8. Пример использует локальный проект,
читает текущего пользователя и, при выборе очереди, до десяти задач. Данные Трекера не меняются.

Bash, из корня репозитория:

```bash
dotnet run --project samples/YandexTracker.QuickStart -- --help
read -rsp 'Токен доступа к Трекеру: ' YANDEX_TRACKER_TOKEN
export YANDEX_TRACKER_TOKEN
export YANDEX_TRACKER_ORG_ID='<organization-id>'
export YANDEX_TRACKER_QUEUE='TEST' # необязательно; без очереди читается только myself
dotnet run --project samples/YandexTracker.QuickStart
unset YANDEX_TRACKER_TOKEN
```

На Windows задайте те же переменные в своей оболочке (например `$env:YANDEX_TRACKER_ORG_ID` в PowerShell)
и выполните те же `dotnet`-команды.

По умолчанию выбраны OAuth и Яндекс 360. Для IAM/Cloud дополнительно задайте
`YANDEX_TRACKER_TOKEN_TYPE=iam` и `YANDEX_TRACKER_ORG_KIND=cloud`. Пример читает только переменные
окружения, не `.env`. Ctrl+C или таймаут в 30 секунд отменяет запросы. Коды выхода: 0 - успех/справка,
1 - ошибка запроса или отмена, 2 - ошибка настройки/аргументов. Вывод содержит данные пользователя и задач.

`dotnet build -c Release` из корня собирает пример без подключения к Трекеру. Подключение через NuGet описано в
[руководстве](../../docs/articles/ru/getting-started.md).
