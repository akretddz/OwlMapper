# Stos technologiczny modułu Account (OwlMapper)

| Technologia / komponent | Rola w module Account |
| --- | --- |
| **C# 14 / .NET 10** | Runtime i język dla warstw Api/Application/Domain/Infrastructure |
| **ASP.NET Core Web API (modularny monolit)** | Ekspozycja endpointów pod bazową ścieżką `account-module`  |
| **Entity Framework Core** | ORM dla Postgres; migracje i mapowanie encji modułu Account |
| **PostgreSQL** | Baza relacyjna dla schematu modułu Account |
| **Migracje EF Core** | Zarządzanie schematem DB modułu; wykonywane przy starcie Bootstrappera |
| **Unit of Work (wspólna implementacja)** | Spójne transakcje dla operacji modułu |
| **Outbox / Inbox (wspólna implementacja)** | Idempotentna publikacja/obsługa komunikatów między modułami |
| **RabbitMQ** | Transport asynchroniczny dla komend/zdarzeń |
| **JWT + Refresh Tokens** | Uwierzytelnianie i autoryzacja użytkowników |
| **Serilog (logowanie strukturalne)** | Korelowane logi żądań i operacji modułu |
| **Redis / cache in-memory** | Cache krótkoterminowy danych odczytowych |
| **NUnit + NSubstitute / Testy Jednostkowe** | Testy jednostkowe per podmoduł: Identity, Management, UserProfile |
| **NUnit + NSubstitute + WebApplicationFactory + Test Containers / Testy Integracyjne** | Testy integracyjne dla modułu
| **NUnit + NSubstitute + WebApplicationFactory + Test Containers / Testy Architektoniczne** | Testy weryfikujące architekturę modułu, konwencje i zależności
| **GitHub Actions / CI/CD** | Pipeline budujący i testujący moduł 