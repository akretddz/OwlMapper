# OwlMapper Bootstrapper

Bootstrapper uruchamia wszystkie moduły systemu OwlMapper (ASP.NET Core 8, modular monolith) i dostarcza wspólne funkcje: konfigurację, logowanie, health-checki oraz ekspozycję punktów API.

## Wymagania
- .NET 8 SDK
- Docker (opcjonalnie, do uruchomienia kontenerowego)
- Dostępne usługi zależne w środowisku:
  - PostgreSQL
  - RabbitMQ

## Konfiguracja
Pliki konfiguracyjne znajdują się w katalogu `src/Bootstrapper/OwlMapper.Bootstrapper`:
- `appsettings.json` – domyślne ustawienia (lokalne)
- `appsettings.local.json` – profil lokalny
- `appsettings.dev.json` – środowisko deweloperskie
- `appsettings.staging.json` – staging
- `appsettings.prod.json` – produkcja
- `appsettings.tests.json` – testy

Dodanie tych plików do konfiguracji ma się odbywać w `Program.cs` w najodpowiedniejszym miejscu

## Uruchomienie lokalne
```bash
# z katalogu repozytorium
dotnet restore
dotnet run --project src/Bootstrapper/OwlMapper.Bootstrapper/OwlMapper.Bootstrapper.csproj
```

Domyślny adres: `https://localhost:2137`.

### Przykładowe żądania (REST Client / curl)
```http
GET `http`/
GET https://localhost:2137/modules
GET https://localhost:2137/health
```

## Profil uruchomieniowy
Plik `Properties/launchSettings.json` definiuje profil `https` i zmienne środowiskowe :
- `ASPNETCORE_ENVIRONMENT = "local"`
- `APPLICATION_NAME = "OwlMapper"`
- `APPLICATION_IDENTIFIER = "owlmapper.bootstrapper"`,
- `applicationUrl = "https://localhost:2137"`

## Health-checki
Zarejestrowane w `Program.cs`:
- `DefaultHealthCheck` – szybki check aplikacji
- `PostgresHealthCheck` – otwarcie połączenia + `select 1`
- `RabbitMqHealthCheck` – próba otwarcia połączenia 

Endpoint: `GET /health`

## Endpointy pomocnicze
- `GET /` – podstawowe info (`Name`, `ApplicationCode`)
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

## Ładowanie modułów
Moduły są wpierw znajdowane wśród wszystkich **assemblies** rozwiązania, 
a potem załadowanie do domeny aplikacji przy użyciu **refleksji**.

Po załadowaniu modułów wstrzykiwane są do nich wymagane dla serwisy.