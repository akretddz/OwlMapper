# Architektura modułu **Account** 

> Dokument opisuje elementy faktycznie obecne w repozytorium. Sekcje niepoparte kodem zostały usunięte lub ograniczone do potwierdzonych informacji.

## Cel i zakres bounded contextu
- **Account** odpowiada za zarządzanie kontami użytkowników: rejestrację, uwierzytelnianie (w tym hasło i MFA), role, uprawnienia, dostawców logowania, tokeny oraz historię zmian.
- Bazowa ścieżka kontraktu modułu: **`/account-module`**.
- Moduł ładowany przez Bootstrapper (ASP.NET Core 10, modularny monolit).

## Podkonteksty (Subcontexts)
Zgodnie z dokumentacją w repo:
- `Identity` – rejestracja, logowanie, aktywacja, hasła, role, MFA.
- `Management` – operacje administracyjne na kontach (blokady, nadawanie ról/uprawnień).
- `UserProfile` – dane profilu użytkownika (np. e-mail, nazwisko, avatar).
- `Security` – dane profilu użytkownika (np. e-mail, nazwisko, avatar).
- `Shared` – kontrakty wspólne (DTO, wyjątki, stałe, rejestracja DI).

### Relacje z innymi modułami

##### Zależności
- Wysyła komendę `SendNotification` do modułu `Notifications`, który w reakcji na nią wysyła powiadomienie e-mail z linkiem do zmiany hasła lub adresu e-mail, albo wyśle link potwierdzający rejestrację konta, lub wyśle wiadomość informującą o odrzuceniu przewoźnika.

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

## Encje (Entities)
Poniżej stan faktyczny na podstawie kodu:

- **Account**: Reprezentuje konto użytkownika
- **IdentityProvider**: Reprezentuje rodzaj uwierzytelniania (hasło/login lub OAuth2)
- **AccountIdentityProvider**: Łącznik `one-to-many` pomiędzy `Account` i `IdentityProvider`
- **AccountToken**: Reprezentuje kod autoryzacyjny użytkownika
- **Role**: Reprezentuje role użytkownika
- **AccountRole**: Łącznik `one-to-many` konto ↔ rola.

## Dane i persystencja

Schemat: `account`

Tabele:
- `accounts`
- `identity_providers`
- `account_identity_providers`
- `account_tokens`
- `roles`
- `account_roles`

Migracje tylko prywatne budujące dany schemat
Encje biznesowe mapują się 1 do 1 na encje bazodanowe

## Bezpieczeństwo, obserwowalność, jakość
- **Autoryzacja / uwierzytelnianie**: systemowo JWT (wg architektury), w module `Account`, `RBAC`
- **Logging / metryki / tracing**: zapewnia Bootstrapper/Shared (Serilog, health-checki); moduł Account nie dostarcza własnych rozszerzeń w widocznych plikach.
- **Walidacja**: brak implementacji w module w udostępnionym kodzie; zalecana walidacja na brzegu (API) i w warstwie logiki API.
- **Idempotencja**: globalnie możliwa przez Outbox/Inbox; brak szczegółów użycia w Account.
- **Caching**: brak użycia w module w widocznym kodzie; globalnie dostępny Redis/in-memory.

