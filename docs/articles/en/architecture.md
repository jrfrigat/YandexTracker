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

The package has no runtime NuGet dependencies.
MinVer and SourceLink are private build dependencies. All public members have English XML documentation.
Models use System.Text.Json; issue extension fields preserve data outside the typed subset.

The SDK implements the Tracker protocol. Application-specific import mapping, configuration and
scheduling belong in the consuming application. Supported endpoints are listed in the README.

Attachments, boards, bulk operations, exports and token acquisition are not implemented.
