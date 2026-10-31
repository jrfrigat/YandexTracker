# Участие в разработке

[English](CONTRIBUTING.md)

Установите SDK .NET 10 и среды выполнения .NET 8/9/10 для запуска всех целевых тестов.

~~~sh
dotnet restore
dotnet build -c Release
dotnet test -c Release --no-build
dotnet pack src/YandexTracker/YandexTracker.csproj -c Release --no-build -o artifacts
dotnet tool restore
dotnet docfx docs/docfx.json --warningsAsErrors
~~~

Группы endpoints делегируют HTTP, авторизацию, повторы и страницы слою Http, а JSON - Serialization.
Публичный API документируется английскими XML-комментариями; отсутствие комментария (CS1591) ломает сборку.
Сохраняйте отмену операций и проверяйте HTTP через StubHandler без настоящих доступов. Необязательные
читающие тесты живого API используют .env.example -> .env. Файл .env не коммитится.

При изменении поведения обновляйте оба языка. docs/api/*.yml и docs/_site генерируются и игнорируются git;
правьте XML-комментарии и Markdown. MinVer берёт версию из vX.Y.Z, без тега создаёт preview.

В PR укажите проблему и проверки. [Настройка выпуска](docs/articles/ru/releasing.md) описывает
первоначальную конфигурацию. Публикация - отдельное действие сопровождающего.
