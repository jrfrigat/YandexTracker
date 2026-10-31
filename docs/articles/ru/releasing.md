# Выпуск

[English](../en/releasing.md)

## Однократная настройка GitHub и NuGet

1. Создайте на GitHub jrfrigat/YandexTracker без начальных файлов. Добавьте URL как origin локального
   репозитория. Проверьте и самостоятельно отправьте рабочую ветку.
2. В Actions variables задайте NUGET_USER - имя аккаунта nuget.org.
3. Создайте environment nuget. На nuget.org настройте Trusted Publisher для пакета: владелец jrfrigat,
   репозиторий YandexTracker, workflow release.yml, environment nuget.
   Используйте актуальное [руководство NuGet](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing).
   Постоянный API-ключ в репозитории не хранится.
4. В Pages -> Build and deployment выберите Source: GitHub Actions; разрешите environment github-pages
   развёртывать main. При желании включите приватные сообщения об уязвимостях.
5. Включите обязательные CI-проверки и защиту ветки. Доступность CodeQL зависит от настроек репозитория/аккаунта.

## Выпуск

Выполните проверки из CONTRIBUTING.ru.md. Обновите оба changelog.
MinVer читает полную историю и теги с префиксом v; коммит без тега получает preview-версию.
Когда нужный коммит попадёт в main, самостоятельно создайте и отправьте тег v0.1.0.
Workflow соберёт и проверит все TFM, упакует библиотеку, получит короткоживущий токен NuGet по OIDC,
опубликует пакет с символами и создаст GitHub release с обоими артефактами.
Тег с prerelease-суффиксом создаёт предварительный релиз, не делая его latest.
Подготовка этих файлов не запускает публикацию и не подтверждает выполнение GitHub Actions.

## Локальный пакет до публикации

PowerShell из этого репозитория:

~~~powershell
$feed = Join-Path $env:TEMP 'yandextracker-local-feed'
dotnet pack src/YandexTracker/YandexTracker.csproj -c Release -p:MinVerVersionOverride=0.1.0 -o $feed
~~~

Для проверки изменённого локального пакета с прежним номером используйте свежий временный каталог
пакетов через --packages при restore и тот же RestorePackagesPath при build/test. Иначе NuGet может
использовать кэшированный 0.1.0. Уже опубликованные стабильные версии не перезаписывайте.

Для подключения пакета в другом проекте добавьте PackageReference на YandexTracker версии 0.1.0.
Восстановите зависимости с локальным источником, заменив путь в примере на файл своего проекта:

~~~powershell
dotnet restore ../Consumer/Consumer.csproj --source $feed --source https://api.nuget.org/v3/index.json
~~~

Чистой машине или CI нужен опубликованный пакет либо доступ к локальному источнику.
Машинозависимые пути источников и бинарники пакетов в git не добавляются.
