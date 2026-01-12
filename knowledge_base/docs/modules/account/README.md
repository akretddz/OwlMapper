# Moduł **Account**

Moduł `Account` odpowiada za zarządzanie kontami użytkowników w architekturze modularnego monolitu OwlMapper. Wspiera rejestrację, uwierzytelnianie, role, profile użytkownika oraz funkcje administracyjne. Bazowa ścieżka kontraktu modułu: **`/account-module`**.

## Podkonteksty

| Podmoduł       | Zakres                                                    | Warstwy (N-layer)                    |
| -------------- | --------------------------------------------------------- | ---------------------------------------------- |
| `Identity`     | Rejestracja, logowanie, aktywacja kont, hasła, role       | - Account.Identity (warstwa prezentacji), - Account.Identity.Core (logika biznesowa + infrastruktura)      |
| `Management`   | Operacje administracyjne / zarządzanie kontami            | Api, Application, Domain, Infrastructure       |
| `UserProfile`  | Dane profilu użytkownika (np. e-mail, nazwisko, avatar)   | Api, Application, Domain, Infrastructure       |
| `Security`  | Usuwanie konta użytkownika, wylogowywanie ze wszystkich sesji | Api, Application, Domain, Infrastructure |
| `Shared`       | Kontrakty wspólne (DTO, wyjątki, stałe)   | Api, Application, Domain, Infrastructure (wg potrzeb) |

## Architektura i integracje

- Styl: **Clean Architecture** per podmoduł (Domain/Application/Infrastructure/Api).
- Ładowanie modułu odbywa się przez Bootstrapper (modularny monolit) – moduł rejestruje swoje serwisy i endpointy poprzez `IModule.Register/Use`.
- Komunikacja asynchroniczna (RabbitMQ + Outbox/Inbox) oraz UoW w infrastrukturze współdzielonej.
- Każdy podmoduł wstyrzykuje swoje niezbędne zależności we własnym zakresie

### Przykładowe zależności domenowe (wg dokumentu `architecture.md`)

- Moduł **wysyła** do `Notifications`: komenda `SendNotification`.
- Moduł **odbiera** z `Notifications`: komenda `SendAccountActivationCode`.
- Moduł **odbiera** zdarzenia z `SagaOrchestration`: `AccountCreated`, `AccountActivationCodeSent`.
- Moduł **odbiera** zdarzenia z `Notifications`: `EmailAddressChanged`, `UserProfileUpdated`.
- Moduł **odbiera** zdarzenie `AccountDeleted` (źródło nieustalone w dokumencie).
- Rekomendacja: utrzymywać kontrakty w warstwie `Shared` i rejestrować je w `ModuleInfo`.

## Konfiguracja

Kluczowe ustawienia znajdują się w plikach `appsettings*.json` Bootstrappera (sekcja `account-module` lub globalne):
- Połączenie z bazą danych PostgreSQL (typowo `database.postgres.connectionString`).
- Konfiguracja messaging (`messaging.messageBusType`, RabbitMQ gdy włączony).
- Cache (Redis, jeśli włączony globalnie).
- Serilog (logowanie ustrukturyzowane).

> Uwaga: doprecyzuj sekcje konfiguracyjne, gdy dodasz własne ustawienia modułowe (np. limity tokenów, polityki haseł).

## Uruchomienie lokalne (przez Bootstrapper)

```bash
dotnet restore
dotnet run --project src/Bootstrapper/OwlMapper.Bootstrapper/OwlMapper.Bootstrapper.csproj
# domyślnie http://localhost:5000, środowisko: local
```

- Endpointy introspekcyjne Bootstrappera: `/`, `/modules`, `/health`, `/errors-introspection`, `/background-services`, `/hubs-introspection`.
- Swagger dostępny, jeśli włączony flagą `swagger.enabled`.

## Migracje bazy danych

1. Dodaj migracje w warstwie `Infrastructure` odpowiedniego podmodułu (np. `Identity`).
2. Upewnij się, że migracje są kopiowane/odpalane zgodnie z konfiguracją Bootstrappera (flagi `autoMigrationEnabled`, `autoSeedDataEnabled`).
3. W środowiskach CI/CD uwzględnij migrator lub start-up automigracji.

## Testy

| Typ testu      | Lokalizacja (wg struktury repo)                |
| -------------- | ---------------------------------------------- |
| Unit           | `tests/Account.UnitTests/` (Shared, Identity, Management, UserProfile) |
| Integration    | `tests/Account.IntegrationTests/`              |
| Architecture   | `tests/OwlMapper.ArchitectureTests/` (globalne reguły) |

Uruchamianie (przykład):
```bash
dotnet test
# lub z filtrem
dotnet test --filter "FullyQualifiedName~Account"
```

## Wersjonowanie i kontrakty API

- Zalecany prefiks wersji: `/api/{client}/v1/account-module/...` (dla BFF web/mobile/admin).
- Kontrakty DTO powinny być stabilne i niezależne od wewnętrznych modeli domenowych.
- Backward compatibility: utrzymuj wersje równoległe; komunikuj deprecacje w nagłówkach.

## Bezpieczeństwo

- JWT Bearer (audience/issuer), minimalny clock skew.
- Polityki ról: `User`, `Carrier`, `Admin` (wg potrzeb frontów).
- Walidacja wejścia na brzegu (API) + ponowna w warstwie aplikacji (defense-in-depth).
- Limity rozmiaru payloadu (domyślne limity Kestrel), CORS per klient (web/mobile/admin).

## Observability

- Strukturalne logi (Serilog) z korelacją żądań.
- Health-checki: Postgres, RabbitMQ, Redis (aktywny zgodnie z konfiguracją).
- Monitorowanie outbox/inbox i usług tła (przez endpointy introspekcyjne Bootstrappera).

## Dobre praktyki implementacyjne

- Trzymaj logikę domenową w warstwie `Domain`, aplikacyjną w `Application`, infrastrukturę w `Infrastructure`, a kontrakty/endpointy w `Api`.
- Mapowanie DTO ↔ modele domenowe w warstwie aplikacji; unikaj przecieków encji domenowych do API.
- Stosuj wzorzec UoW i Outbox/Inbox (już dostępne we wspólnej infrastrukturze).
- Dodawaj testy jednostkowe do reguł domenowych (hasła, role, tokeny) i testy integracyjne do ścieżek API.

## TODO / miejsca do uzupełnienia

- Lista konkretnych endpointów API per podmoduł (`Identity`, `Management`, `UserProfile`).
- Przykładowe request/response (JSON) i kody odpowiedzi.
- Konfiguracja polityk haseł, TTL tokenów, refresh tokens.
- Schemat bazy danych (tabele i relacje) po wygenerowaniu migracji.