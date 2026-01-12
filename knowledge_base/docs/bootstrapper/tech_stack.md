# Stos technologiczny Bootstrappera (Evaround)

| Technologia / komponent | Rola w Bootstrapperze |
| --- | --- |
| **C# 14 / .NET 10 (ASP.NET Core WebHost)** | Host aplikacji modularnego monolitu; kompozycja modułów, pipeline HTTP, endpointy wspólne |
| **Kestrel** | Serwer HTTP, limity (MaxRequestBodySize 60 MB, Timeout 2 min) |
| **ASP.NET Core (Startup/Program)** | Rejestracja usług i middleware, ładowanie modułów (`ModuleLoader`), konfiguracja wersjonowania, `Scalar`, health-checki |
| **Health Checks** | `DefaultHealthCheck`, `PostgresHealthCheck` (`Dapper` + `Npgsql`, `select 1`), `RabbitMqHealthCheck` (RabbitMQ.Client), `RedisHealthCheck` (StackExchange.Redis) – endpoint `/health` |
| **Konfiguracja (appsettings.\*.json + Consul)** | Profile środowiskowe (dev/staging/prod/tests); kluczowe sekcje: `database`, `messaging`, `caching.redis`, `serilog` |
| **Serilog** | Strukturalne logowanie (Console, Seq), wzbogacenie o `ApplicationCode`; konfiguracja z plików `appsettings.*` |
| **Scalar** | Dokumentacja API (włączana flagą `scalar.enabled`), UI w pipeline |
| **Messaging** | `RabbitMQ`, `Wolverine`, integracja z `Outbox/Inbox` |
| **Baza danych** | `PostgreSQL` (`Npgsql`, `EF Core` w modułach), automigracja/seed sterowane flagami `autoMigrationEnabled` / `autoSeedDataEnabled` |
| **Cache** | `Redis` lub cache w pamięci (wg `caching.redis.enabled`) |
| **Observability / Introspekcja** | Endpointy: `/modules`, `/errors-introspection`, `/background-services`, `/hubs-introspection`; korelowane logi |
| **Docker** | Wieloetapowy build (SDK → runtime) w `src/Bootstrapper/Evaround.Bootstrapper/Dockerfile`, obraz ASP.NET Core 10 |
| **REST Client examples** | Plik `Evaround.rest` z przykładowymi żądaniami: `/`, `/modules`, `/health` |