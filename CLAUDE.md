# YandexTracker

Read C:/Users/frost/.claude/CLAUDE.md before work. This is an independent SDK for .NET 8/9/10.
Read README.md, CONTRIBUTING.md and docs/articles/en/architecture.md for the project boundaries.
Public API needs English XML comments. Keep both documentation languages current.
SDK protocol work belongs here; Alva import mapping belongs in the linked Alva project.
Use Aiko as durable workflow: read project context/card/feed, start its stage before edits, produce
required artifacts through Aiko, refresh readiness, sign the stage result as your adapter, then complete.
Create a card for new requests; execute it only when the user asks to run it or asks for card and work together.
Read global commit/branch rules; never push or publish without explicit authorization.