# Contributing

[Русский](CONTRIBUTING.ru.md)

Install the .NET 10 SDK and .NET 8/9/10 runtimes to execute every test target.

~~~sh
dotnet restore
dotnet build -c Release
dotnet test -c Release --no-build
dotnet pack src/YandexTracker/YandexTracker.csproj -c Release --no-build -o artifacts
dotnet tool restore
dotnet docfx docs/docfx.json --warningsAsErrors
~~~

Keep endpoint groups thin. HTTP/authentication/retries/pagination belong in Http, JSON behavior in
Serialization. Public types need English XML comments; CS1591 fails the build. Preserve cancellation
and test HTTP behavior using StubHandler without live credentials. For optional read-only live tests,
copy .env.example to .env and fill in your own access details. Never commit that file.

Update both language guides when behavior changes. Generated docs/api/*.yml and docs/_site are ignored;
edit C# XML comments or Markdown instead. Version tags vX.Y.Z drive MinVer; untagged builds are previews.

Submit a focused pull request with validation results. See [release setup](docs/articles/en/releasing.md)
for the one-time repository configuration. Publishing is a separate maintainer action.
