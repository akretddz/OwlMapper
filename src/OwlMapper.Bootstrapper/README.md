# OwlMapper Bootstrapper

Główny punkt wejściowy aplikacji OwlMapper, który zarządza ładowaniem modułów, konfiguracją i dostępnością aplikacji.

## Funkcje

- **Ładowanie modułów**: Dynamiczne ładowanie i rejestracja modułów implementujących interfejs `IModule`
- **Konfiguracja per środowisko**: Obsługa środowisk: Development, Staging, Production, Tests
- **Health Checks**: Monitorowanie dostępności aplikacji, PostgreSQL i RabbitMQ
- **Serilog**: Logowanie z możliwością wysyłania logów do Seq
- **Zmienne środowiskowe**: Wyświetlanie `APPLICATION_NAME` i `APPLICATION_IDENTIFIER`

## Uruchomienie

```bash
cd src/OwlMapper.Bootstrapper
dotnet run
```

Aplikacja będzie dostępna pod adresem: `https://localhost:2137`

## Endpointy

### Root `/`
Zwraca informacje o aplikacji w formacie JSON:
```json
{
  "APPLICATION_NAME": "OwlMapper",
  "APPLICATION_IDENTIFIER": "bootstrapper"
}
```

### Health Check `/health`
Zwraca status aplikacji:
- `Healthy` - aplikacja działa poprawnie
- `Unhealthy` - występują problemy (np. brak połączenia z bazą danych lub RabbitMQ)

**Uwaga**: Healthchecki dla PostgreSQL i RabbitMQ będą poprawnie działać dopiero po konfiguracji w zadaniu #4.

## Zmienne środowiskowe

- `APPLICATION_NAME` - nazwa aplikacji (domyślnie: "OwlMapper")
- `APPLICATION_IDENTIFIER` - identyfikator aplikacji (domyślnie: "bootstrapper")
- `ASPNETCORE_ENVIRONMENT` - środowisko aplikacji (Development/Staging/Production/Tests)

## Konfiguracja środowisk

Aplikacja używa plików `appsettings.{Environment}.json` do konfiguracji per środowisko:

- `appsettings.json` - konfiguracja bazowa
- `appsettings.Development.json` - konfiguracja dla środowiska deweloperskiego
- `appsettings.Staging.json` - konfiguracja dla środowiska testowego
- `appsettings.Production.json` - konfiguracja produkcyjna
- `appsettings.Tests.json` - konfiguracja dla testów

## Moduły

Moduły są ładowane automatycznie przez Bootstrapper. Każdy moduł musi:
1. Implementować interfejs `IModule` z `OwlMapper.Shared`
2. Implementować metodę `RegisterServices` do rejestracji serwisów w DI
3. Implementować metodę `ConfigureApplication` do konfiguracji pipeline aplikacji

Moduły można włączać/wyłączać w pliku `appsettings.json`:

```json
{
  "Modules": {
    "NazwaModułu": {
      "Enabled": true
    }
  }
}
```

## Serilog

Serilog jest skonfigurowany do logowania do:
- Console (zawsze)
- Seq (opcjonalnie, w środowiskach Staging/Production)

Konfiguracja Seq znajduje się w odpowiednich plikach `appsettings.{Environment}.json`.

## Health Checks

Aplikacja monitoruje:
- **default** - ogólna dostępność aplikacji
- **postgres** - połączenie z bazą danych PostgreSQL
- **rabbitmq** - połączenie z RabbitMQ

## Wymagania

- .NET 9.0 SDK
- PostgreSQL (opcjonalnie, dla health checków)
- RabbitMQ (opcjonalnie, dla health checków)
- Seq (opcjonalnie, dla logowania w Staging/Production)
