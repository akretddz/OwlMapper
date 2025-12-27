# Aplikacja - OwlMapper 
Platforma do wyszukiwania połączeń autobusowych oraz do zarządzania rozkładami jazdy.

## Architektura

Aplikacja wykorzystuje architekturę modułową z centralnym punktem wejściowym - **Bootstrapperem**.

### Bootstrapper

Bootstrapper jest odpowiedzialny za:

- Ładowanie konfiguracji per środowisko (dev, staging, prod, tests)
- Automatyczne wykrywanie i ładowanie modułów
- Rejestrację serwisów w kontenerze DI
- Konfigurację health checks (PostgreSQL, RabbitMQ)
- Logowanie przez Serilog do Seq
- Obsługę zmiennych środowiskowych (`ApplicationIdentifier`, `ApplicationName`)

Szczegółowa dokumentacja: [src/Bootstrapper/README.md](src/Bootstrapper/README.md)

## Wymagania

- .NET 10.0
- PostgreSQL (opcjonalnie)
- RabbitMQ (opcjonalnie)
- Seq (opcjonalnie - dla centralnego logowania)

## Uruchomienie

```bash
cd src/Bootstrapper
dotnet run
```

### Zmienne środowiskowe

```bash
ApplicationIdentifier="my-instance" ApplicationName="OwlMapper" dotnet run
```

## Endpointy

- `GET /` - Informacje o aplikacji
- `GET /health` - Health check systemu
- `GET /example` - Przykładowy endpoint z modułu (ExampleModule)

## Struktura Projektu

```
├── src/
│   └── Bootstrapper/          # Główna aplikacja
│       ├── Models/            # Modele danych
│       ├── Modules/           # Moduły aplikacji
│       └── README.md          # Dokumentacja Bootstrappera
├── OwlMapper.sln              # Solution
└── README.md                  # Ten plik
```

