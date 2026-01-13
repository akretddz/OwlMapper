## Architektura Bootstrappera

### Warstwa hosta (Bootstrapper)
- **ASP.NET Core 10 WebHost** – pojedyncza aplikacja uruchamiająca wszystkie moduły.
- **Middleware i endpointy wspólne**:
  - `/` – podstawowe informacje o aplikacji (`ApiName`, `ApplicationCode`)
  - `/modules` – lista załadowanych modułów
  - `/health` – health-checki
  - `Scalar UI` (gdy włączony)
- **Rejestracja modułów**: `ModuleLoader.LoadAssemblies/LoadModules`, następnie `module.Register` w DI i `module.Use` w pipeline.

### Moduły
- Bootstrapper referencjonuje moduły w `Bootstrapper.csproj` i ładuje je dynamicznie.
- Moduły rejestrują swoje serwisy, endpointy i middleware poprzez interfejs `IModule`.

### Warstwa współdzielona (Shared)
- **Konfiguracja**: pliki `appsettings*.json` (dev/staging/prod/tests) kopiowane do outputu.
- **Logowanie**: Serilog (Console, Seq), wzbogacenie o `ApplicationCode`.
- **API**: wersjonowanie, Scalar 
- **Baza danych**: PostgreSQL, możliwość automigracji i seed
- **Messaging**: RabbitMQ outbox/inbox.
- **Cache**: Redis lub in-memory.

### Health-checki
- `DefaultHealthCheck` – podstawowy check aplikacji.
- `PostgresHealthCheck` – otwarcie połączenia + `select 1`.
- `RabbitMqHealthCheck` – próba połączenia
- `RedisHealthCheck` – `PING`
- Wystawione pod `/health`.
  
Health-checki respektują flagi konfiguracyjne.

### Konfiguracja środowiskowa
- `appsettings.json` (lokalne domyślne) 
- `appsettings.dev.json`
- `appsettings.staging.json`
- `appsettings.prod.json`
- `appsettings.tests.json`
  
Kluczowe sekcje: 
- `database.postgres.connectionString`
- `messaging`
- `serilog`

### Budowanie i uruchamianie
- **Lokalnie**: `dotnet restore` → `dotnet run --project src/Bootstrapper/Bootstrapper.csproj` (domyślnie `http://localhost:5000`, profil `ASPNETCORE_ENVIRONMENT=local`).
- **Docker**: wieloetapowy build (SDK → runtime) w `Dockerfile`, kontener nasłuchuje na porcie 80.

### Dane zależne (wewnętrzne oczekiwania)
- Postgres (DB) 
- RabbitMQ (gdy włączony) 
- Wszystkie parametry definiowane w `appsettings*.json`

