# Architecture

[Русский](../ru/architecture.md)

| Area | Responsibility |
| --- | --- |
| YandexTrackerClient / IYandexTrackerClient | Entry point, endpoint groups, HTTP ownership |
| Endpoints | Public interfaces and internal request adapters |
| Models | Wire models and extensible issue data |
| Http | Authentication, requests, retries, pagination, API errors |
| Serialization | JSON options and Tracker timestamp format |
| Exceptions | API status types and response deserialization errors |
| YandexTrackerDuration | Working-duration conversion |

No Alva or ASP.NET dependency is present. The package has no runtime NuGet dependencies.
MinVer and SourceLink are private build dependencies. All public members have English XML documentation.
Models use System.Text.Json; issue extension fields preserve data outside the typed subset.

Alva consumes the package through Alva.Integrations.YandexTracker. That module owns import mapping,
credentials/configuration handling, external identifiers and dependency conversion. The SDK owns the
Tracker protocol. File SDK requests in the YandexTracker Aiko project and Alva behavior in Alva.

This implementation covers a useful subset of the official Python client, not its entire API.
Attachments, boards, bulk operations, exports and token acquisition are not implemented.
