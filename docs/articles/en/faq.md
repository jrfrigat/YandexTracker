# FAQ

[Русский](../ru/faq.md)

## Is this an official SDK or a complete Python SDK port?
No. This is an independent .NET client with the endpoint subset listed in the README.
Use the [official API reference](https://yandex.ru/support/tracker/ru/api/about-api) for service behavior.

## Why are four tests skipped?
The read-only live checks need YANDEX_TRACKER_TOKEN and YANDEX_TRACKER_ORG_ID.
Unit tests always run without network access. The repository root .env is read by tests only.
No live credentials are provided by CI.

## Why does 0.1.0 fail to restore?
It must first be published to NuGet. Until then pack this repository and use a temporary local feed
as described in [Releasing and Alva](releasing.md). Do not commit local paths or package binaries.

## How do I inspect custom fields?
Issue.Fields holds JSON extension properties. Read the JsonElement using the actual field schema.

## Is Native AOT supported?
It has not been validated. Serialization uses reflection; do not assume AOT/trimming compatibility.

## Where do I request changes?
SDK API behavior belongs in YandexTracker. Alva's mapping, UI and scheduling belong in Alva.
