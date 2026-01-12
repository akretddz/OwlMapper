# OwlMapper Bootstrapper

Bootstrapper uruchamia wszystkie moduły systemu OwlMapper (ASP.NET Core 8, modular monolith) i dostarcza wspólne funkcje: konfigurację, logowanie, health-checki oraz ekspozycję punktów API.

## Wymagania
- .NET 8 SDK
- Docker (opcjonalnie, do uruchomienia kontenerowego)
- Dostępne usługi zależne w środowisku:
  - PostgreSQL
  - RabbitMQ 
  - Redis
  - Consul 

## Konfiguracja
Pliki konfiguracyjne znajdują się w katalogu `src/Bootstrapper/OwlMapper.Bootstrapper`:
- `appsettings.json` – domyślne ustawienia (lokalne)
- `appsettings.local.json` – profil lokalny
- `appsettings.dev.json` – środowisko deweloperskie
- `appsettings.staging.json` – staging
- `appsettings.prod.json` – produkcja
- `appsettings.tests.json` – testy

> Wszystkie pliki `appsettings.*.json` są kopiowane do outputu dzięki konfiguracji w `OwlMapper.Bootstrapper.csproj`.

## Uruchomienie lokalne
```bash
# z katalogu repozytorium
dotnet restore
dotnet run --project src/Bootstrapper/OwlMapper.Bootstrapper/OwlMapper.Bootstrapper.csproj
```

Domyślny adres: `http://localhost:5000`.

### Przykładowe żądania (REST Client / curl)
```http
GET http://localhost:5000/
GET http://localhost:5000/modules
GET http://localhost:5000/health
```

## Profil uruchomieniowy
Plik `Properties/launchSettings.json` definiuje profil `http`:
- `ASPNETCORE_ENVIRONMENT=local`
- `ApiName=OwlMapper`
- `ApplicationCode=owlmapper.api.core`
- `applicationUrl=http://localhost:5000`

## Health-checki
Zarejestrowane w `Startup`:
- `DefaultHealthCheck` – szybki check aplikacji
- `PostgresHealthCheck` – otwarcie połączenia + `select 1`
- `RabbitMqHealthCheck` – próba otwarcia połączenia 
- `RedisHealthCheck` – `PING`

Endpoint: `GET /health`

## Endpointy pomocnicze
- `GET /` – podstawowe info (`ApiName`, `ApplicationCode`)
- `GET /modules` – lista modułów
- `GET /errors-introspection` – introspekcja błędów
- `GET /background-services` – introspekcja usług tła
- `GET /hubs-introspection` – introspekcja hubów
- `GET /health` – health-checki

## Docker
Obraz budowany jest wieloetapowo (SDK + runtime) w `Dockerfile`.
```bash
docker build -t owlmapper-bootstrapper -f src/Bootstrapper/OwlMapper.Bootstrapper/Dockerfile .
docker run -p 5000:80 owlmapper-bootstrapper
```