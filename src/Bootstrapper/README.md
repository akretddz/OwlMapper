# OwlMapper Bootstrapper

Punkt wejściowy aplikacji OwlMapper, odpowiedzialny za ładowanie konfiguracji, modułów oraz inicjalizację wszystkich komponentów systemu.

## Funkcjonalności

### 1. Ładowanie Konfiguracji

Bootstrapper ładuje konfigurację z plików `appsettings.json` w zależności od środowiska:

- **Development** (`appsettings.Development.json`) - środowisko deweloperskie
- **Staging** (`appsettings.Staging.json`) - środowisko stagingowe
- **Production** (`appsettings.Production.json`) - środowisko produkcyjne
- **Tests** (`appsettings.Tests.json`) - środowisko testowe

### 2. Moduły

System wykorzystuje architekturę modułową. Moduły są automatycznie wykrywane i ładowane podczas startu aplikacji.

#### Tworzenie Modułu

Aby stworzyć nowy moduł, należy zaimplementować interfejs `IModule`:

```csharp
public class MyModule : IModule
{
    public string Name => "MyModule";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja serwisów w DI
        services.AddScoped<IMyService, MyService>();
    }

    public void UseModule(IApplicationBuilder app)
    {
        // Dodanie modułu do pipeline
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/my-endpoint", () => "Hello from MyModule");
        });
    }
}
```

#### Konfiguracja Modułów

Moduły mogą być włączane/wyłączane przez konfigurację w pliku `appsettings.json`:

```json
{
  "Modules": {
    "EnabledModules": {
      "MyModule": true,
      "AnotherModule": false
    }
  }
}
```

### 3. Zmienne Środowiskowe

Aplikacja obsługuje następujące zmienne środowiskowe:

- `ApplicationIdentifier` - unikalny identyfikator instancji aplikacji
- `ApplicationName` - nazwa aplikacji

Wartości te nadpisują ustawienia z pliku konfiguracyjnego.

### 4. Health Checks

Aplikacja udostępnia endpoint `/health` z informacjami o stanie systemu:

- **Default** - podstawowy health check
- **PostgreSQL** - sprawdzanie połączenia z bazą danych
- **RabbitMQ** - sprawdzanie połączenia z kolejką komunikatów

Konfiguracja health checks:

```json
{
  "HealthChecks": {
    "EnablePostgreSQL": true,
    "EnableRabbitMQ": true
  },
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Port=5432;Database=owlmapper;Username=postgres;Password=postgres",
    "RabbitMQ": "amqp://guest:guest@localhost:5672/"
  }
}
```

### 5. Logowanie - Serilog

Aplikacja wykorzystuje Serilog do logowania z następującymi sinkami:

- **Console** - wyświetlanie logów w konsoli
- **File** - zapisywanie logów do plików w katalogu `logs/`
- **Seq** - wysyłanie logów do Seq (konfigurowalny adres)

Konfiguracja Seq:

```json
{
  "Seq": {
    "ServerUrl": "http://localhost:5341",
    "ApiKey": ""
  }
}
```

## Endpointy

### Informacje o Aplikacji

```
GET /
```

Zwraca informacje o aplikacji w formacie JSON:

```json
{
  "applicationIdentifier": "owlmapper-dev",
  "applicationName": "OwlMapper Development"
}
```

### Health Check

```
GET /health
```

Zwraca szczegółowe informacje o stanie systemu:

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "postgres",
      "status": "Healthy",
      "description": null,
      "duration": "00:00:00.0234567",
      "exception": null,
      "data": {}
    }
  ],
  "totalDuration": "00:00:00.0345678"
}
```

## Uruchomienie

### Lokalne

```bash
cd src/Bootstrapper
dotnet run
```

### Z wyborem środowiska

```bash
ASPNETCORE_ENVIRONMENT=Staging dotnet run
```

### Ze zmiennymi środowiskowymi

```bash
ApplicationIdentifier="my-instance-1" ApplicationName="My App" dotnet run
```

## Struktura Projektu

```
src/Bootstrapper/
├── Models/
│   └── ApplicationInfo.cs          # Model informacji o aplikacji
├── Modules/
│   ├── IModule.cs                  # Interfejs modułu
│   ├── ModuleConfiguration.cs      # Konfiguracja modułów
│   ├── ModuleLoader.cs             # Loader modułów
│   └── Examples/
│       └── ExampleModule.cs        # Przykładowy moduł
├── Program.cs                      # Punkt wejściowy aplikacji
├── appsettings.json                # Konfiguracja bazowa
├── appsettings.Development.json    # Konfiguracja dev
├── appsettings.Staging.json        # Konfiguracja staging
├── appsettings.Production.json     # Konfiguracja prod
└── appsettings.Tests.json          # Konfiguracja testowa
```

## Wymagania

- .NET 10.0
- PostgreSQL (opcjonalnie)
- RabbitMQ (opcjonalnie)
- Seq (opcjonalnie)
