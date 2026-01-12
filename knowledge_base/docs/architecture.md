## Architektura backendu OwlMapper

Modularny monolit oparty jest o kontrakt `ModuleManifest` (rejestracja + pipeline), wspólny projekt **Shared**, transport asynchroniczny (RabbitMQ + Outbox/Inbox), UoW per moduł oraz separację kodu według warstw.
Każdy moduł znajduje się w odpowiednim folderze znajdującym się pod ścieżką `./src/modules`

### Moduły

| Moduł | Styl architektury | Uwagi / ścieżka bazowa |
| --- | --- | --- |
| Account | Architektura dwuwarstwowa | BasePath `account-module`; ma podmoduły (poniżej) |
| RoutePlanner | Clean Architecture | BasePath `routeplanner-module` |
| Timetable | Clean Architecture | BasePath `timetable-module` |
| BusStops | Clean Architecture | BasePath `busstops-module` |
| Places | Clean Architecture | BasePath `places-module` |
| Notifications | Clean Architecture | BasePath `notifications-module`; ma podmoduły (poniżej) |
| LineVariants | Clean Architecture | BasePath `linevariants-module`; ma podmoduły (poniżej) |
| Journeys | Clean Architecture | BasePath `journeys-module` |
| SagaOrchestration | Clean Architecture | BasePath `sagaorchestration-module` |

### Podmoduły

- **Account**: `Identity`, `Management`, `UserProfile`
- **Notifications**: `Audience`, `Delivery`, `Templates`
- **LineVariants**: `Trips`

### Relacje między modułami

#### 1. Account
`Account` -> `Notifications`   
`Notifications` -> `Account`

##### Zależności
- Do modułu `Notifications` wysyła komendę `SendNotification` 

##### Kontrakty
- Do modułu `SagaOrchestration` wysyła zdarzenia:
	* `AccountCreated`
	* `AccountActivationCodeSent`
- Do modułu `Notifications` wysyła zdarzenia:
	* `EmailAddressChanged`
	* `UserProfileUpdated`
	* `AccountDeleted`
- Z modułu `SagaOrchestration` przychodzi komenda:
	* `SendAccountActivationCode`
	
#### 2. Journeys
`Journeys` -> `Notifications`

Wysyła: 
- Do modułu `Notifications` komendę `SendNotification` 

#### 3. LineVariants
`LineVariants` -> `Notifications`
`LineVariants` -> `BusStops`
`RoutePlanner` -> `LineVariants`
`Timetable`    -> `LineVariants`

Wysyła: 
- Do modułu `Notifications` komendę `SendNotification` 
- Do modułu `BusStops` zdarzenie `BusStopPublished` 
Odbiera: 
- Z modułu `RoutePlanner` zdarzenia:
	* `LineVariantPublished`
	* `LineVariantDeleted`
- Z modułu `SagaOrchestration` zdarzenia:
	* `BusStopPossibleToDelete`
	* `BusStopImpossibleToDelete`
- Z modułu `SagaOrchestration` komendę: `DeleteBusStopsWithLineVariants`
- Z modułu `Timetable` zdarzenia:
    * `LineDeleted`
	* `LineModified`
	* `LineVariantPublished`
	* `LineVariantDeleted`
	* `DefinedTimetableForLineVariant` ## BRAK PO PRAWEJ STRONIE KONTEKSTU LINE VARIANTS 

#### 4. RoutePlanner
`RoutePlanner` -> `LineVariants`
`RoutePlanner` -> `BusStops`

Wysyła: 
- Do modułu `LineVariants` zdarzenia:
	* `LineVariantPublished`
	* `LineVariantDeleted`
	* `DefinedTimetableForLineVariant`
- Do modułu `BusStops` zdarzenia:
	* `BusStopPublished`
	* `BusStopDeleted`
	* `BusStopModified`
	
#### 5. Timetable
`Timetable` -> `LineVariants`
`Timetable` -> `BusStops`

Wysyła: 
- Do modułu `LineVariants` zdarzenia:
	* `LineVariantPublished`
	* `LineVariantDeleted`
	* `DefinedTimetableForLineVariant`
	* `LineDeleted`
	* `LineModified`
- Do modułu `BusStops` zdarzenia:
	* `BusStopPublished`
	* `BusStopDeleted`
	* `BusStopModified`
	
#### 6. BusStops
`Timetable` -> `BusStops`
`RoutePlanner` -> `BusStops`
`LineVariants` -> `BusStops`

Odbiera: 
- Z modułu `Timetable` zdarzenia:
	* `BusStopPublished`
	* `BusStopDeleted`
	* `BusStopModified`
- Z modułu `RoutePlanner` zdarzenia:
	* `BusStopPublished`
	* `BusStopDeleted`
	* `BusStopModified`
- Z modułu `SagaOrchestration` zdarzenia:
	* `BusStopMarkedToDelete`
	* `BusStopMarkedAsActive`
- Z modułu `SagaOrchestration` komendy:
	* `DeleteBusStopPermanently`
	* `UndoBusStopRemoving`
	
#### 7. Places
Brak zależności

#### 8. Notifications
`LineVariants` -> `Notifications`
`Journeys` -> `Notifications`
`Account` -> `Notifications`
`Notifications` -> `Account`

Wysyła:
- Do modułu `Account` zdarzenie `EmailAddressChanged`
- Do modułu `Account` zdarzenie `UserProfileUpdated`
Odbiera: 
- Z modułu `LineVariants` komendę `SendNotification`
- Z modułu `Journeys` komendę `SendNotification`
- Z modułu `Account` komendę `SendNotification`
	
### Wzorce komunikacji (Events / Commands)

- Eventy i asynchroniczne komendy oznaczone atrybutem kontraktu (`[Contract]`), typy implementują `IEvent` lub `IAsyncCommand`; są rejestrowane do `ModuleInfo` (Shared.Infrastructure Modules/Registration).
- Przykładowe eventy: `AccountCreated` (Account), `EmailAddressChanged`, `AccountActivated`, `AccountDeleted` (Account → konsumowane w Guide).
- Przykładowa komenda: `RecordAuditError` (Audit).

### Pozostałe projekty

- **Shared**: kontrakty, interfejsy, typy bazowe (Modules, Messaging, Saga, Kernel, implementacje transportu (RabbitMQ, Outbox/Inbox), rejestracja modułów (`ModuleRegistry`, `ModuleClient`, `ModuleSubscriber`), UoW, konwencje, integracje (Consul, Seq), migratory baz, cache/Redis, itp.
- **Bootstrapper**: host aplikacji (Dockerfile, kompozycja wszystkich modułów, kontenery infrastruktury).
- **Clients/Web** (Angular) i **Clients/Mobile**: aplikacje klienckie (BFF/web API + mobile API wspomniane w `docs/architecture.md`).
- **Tests**:
  - `tests/Evaround.Tests.Architecture`: testy reguł architektonicznych (Clean Architecture, naming, separacja warstw, brak zależności między modułami).
  - `tests/Evaround.Tests.Integration`: testy integracyjne per moduł (uruchamiane z filtrem `OwlMapper.Tests.Integration.<Module>`; wspomagane skryptem `deployment/scripts/run_integration_tests.sh`).
  - `tests/Unit/...`: testy jednostkowe, również testy współdzielone (stuby klienta modułów, narzędzia do sag).
  - `tests/OwlMapper.Tests.Shared`: stałe testowe, stuby (np. `StubModuleClient`), narzędzia wspólne.

### Dane i persystancja

Każdy moduł ma swój `dbContext`, swój schemat bazy danych i swoje tabele bazodanowe i co za tym idzie swoje własne encje.

### Krótka charakterystyka runtime

- Każdy moduł definiuje `BasePath` (`{modul}-module`) i rejestruje swoje usługi, UoW, Outbox/Inbox oraz subskrypcje RabbitMQ w `Use(...)`.
- Moduły są ładowane dynamicznie; `ModuleInfoContainer` zbiera informacje o kontraktach (eventy/komendy) do introspekcji (`GET /modules`).
- Warstwa integracji korzysta z Postgres, Redis, RabbitMQ, Seq, Consul; aplikacja konteneryzowana (Docker).

> Uwaga: aby uzyskać pełną, wyczerpującą listę encji, eventów i komend per moduł, trzeba przeglądnąć katalogi `Domain`/`Core` oraz `Application` każdego modułu; w dostarczonym materiale mamy tylko fragmenty rejestracji i kontraktów.