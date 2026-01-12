# Zdarzenia modułu Account

Poniższe zestawienie obejmuje wszystkie zdarzenia używane w module **Account**:
- **prywatne (domenowe)** – tylko wewnątrz modułu,
- **wychodzące (integracyjne)** – publikowane na zewnątrz,
- **przychodzące (integracyjne)** – konsumowane z innych modułów.

---

## 2. Prywatne zdarzenia domenowe (internal)

| Zdarzenie | Payload | Kiedy powstaje | Główni handlerzy / efekty |
| --- | --- | --- | --- |
| `AccountCreated`: publikuje async komendę `CreateRecipient` do modułu Notifications (utworzenie odbiorcy).<br>- Mapper → integracja `UserCreated`. |
| `AccountConfirmed(Account Account)` | pełne konto | Potwierdzenie e-mail (kod lub admin) | - `AccountConfirmedHandler`: wysyła powitalną notyfikację (jeśli brak zewn. providera).<br>- Mapper → integracja `UserConfirmed`. |
| `AccountDeleted(string AccountId)` | identyfikator konta | Usunięcie konta | Mapper → integracja `UserDeleted`. |
| `AccountEmailAddressChanged(Account Account)` | konto z nowym e-mailem | Potwierdzenie zmiany adresu e-mail | Mapper → integracja `EmailAddressChanged`. |
| `AccountUsernameChanged(Account Account)` | konto z nową nazwą użytkownika | Zmiana nazwy użytkownika | Mapper → integracja `UsernameChanged`. |
| `ExternalLoginProviderConnected(string AccountId, ExternalLoginResult ExternalLoginResult)` | konto, dane providera (username, photo) | Podpięcie zewnętrznego loginu | Mapper → integracja `ExternalLoginProviderConnected`. |
| `ChangeEmailAddressTokenGenerated(Account Account, string NewEmailAddress, string Token)` | konto, nowy e-mail, token | Żądanie zmiany e-mail (etap 1) | `ChangeEmailAddressTokenGeneratedHandler`: wysyła notyfikację ręczną z tokenem. |
| `AccountChangeRegistered(AccountChange AccountChange)` | wpis audytu zmiany konta | Rejestrowanie zmian (np. profil, role) | `AccountChangeRegisteredHandler`: zapis do repo zmian + push do `AccountChangesHub`. |

---

## 3. Zdarzenia wychodzące (integracyjne, publikowane przez moduł Account)

| Zdarzenie | Payload | Źródło (mapowane z) | Semantyka |
| --- | --- | --- | --- |
| `UserCreated(string UserId, string EmailAddress, string? Username, string? LanguageCode)` | Id konta, e-mail, opcjonalny username i język | `AccountCreated` | Konto utworzone (niezależnie od potwierdzenia). |
| `UserConfirmed(string UserId, string EmailAddress, string? Username)` | Id konta, e-mail, username | `AccountConfirmed` | Konto zostało potwierdzone. |
| `UserDeleted(string UserId)` | Id konta | `AccountDeleted` | Konto usunięte. |
| `EmailAddressChanged(string UserId, string EmailAddress)` | Id konta, nowy e-mail | `AccountEmailAddressChanged` | E-mail zmieniony i potwierdzony. |
| `UsernameChanged(string UserId, string? Username)` | Id konta, nowy username | `AccountUsernameChanged` | Nazwa użytkownika zmieniona. |
| `ExternalLoginProviderConnected(string UserId, string? Username, string? PhotoUrl)` | Id konta, dane z providera | `ExternalLoginProviderConnected` | Podpięto zewnętrznego dostawcę logowania. |

> Wszystkie powyższe są publikowane z Outbox (po `AccountDomainEventMapper`) i oznaczone `[Contract]`.

Dodatkowe wyjście (asynchroniczna komenda do innego modułu):
- `CreateRecipient(string UserId, string EmailAddress, string? Username, string? LanguageCode)` (`IAsyncCommand`, `[Contract]`) – wysyłane do modułu Notifications po `AccountCreated` w celu utworzenia odbiorcy powiadomień.

---

## 4. Zdarzenia przychodzące (integracyjne, konsumowane przez moduł Account)

| Zdarzenie | Źródło | Użycie w Account |
| --- | --- | --- |
| `RecipientCreated(string RecipientId)` | Moduł Notifications | Subskrypcja w `AccountModule.Use(...).UseRabbitMq().SubscribeEvent<RecipientCreated>()`. (Aktualny kod nie pokazuje handlera – zdarzenie dostępne do konsumpcji, np. do synchronizacji stanu odbiorców). |

> Z dokumentu architektonicznego moduł ma przewidziane dodatkowe wejścia (`AccountCreated`, `AccountActivationCodeSent` z SagaOrchestration; `EmailAddressChanged`, `UserProfileUpdated` z Notifications; `AccountDeleted` zewnętrzne), jednak w aktualnym kodzie modułu Account jedyną jawną subskrypcją jest `RecipientCreated`. Po dodaniu handlerów należy dopisać je do tej listy.

---

## 5. Przepływ publikacji i przetwarzania

1. **Generacja zdarzenia domenowego** – np. `AccountCreated` podczas rejestracji.
2. **Handler domenowy** wykonuje akcje lokalne (notyfikacje, audyt) i/lub publikuje komendę asynchroniczną (`CreateRecipient`).
3. **Mapowanie na zdarzenie integracyjne** przez `AccountDomainEventMapper`; obiekt trafia do Outbox.
4. **Transport** – Outbox → (opcjonalnie) RabbitMQ; kontrakty oznaczone `[Contract]`.
5. **Konsumpcja przychodzących** – `UseRabbitMq().SubscribeEvent<RecipientCreated>()` umożliwia odbiór zdarzeń z modułu Notifications.

---

## 6. Notatki utrzymaniowe
- Dodając nowe zdarzenia domenowe, zaktualizuj `AccountDomainEventMapper`, aby były publikowane na zewnątrz (jeśli to kontrakt publiczny).
- Dla nowych zdarzeń przychodzących dodaj subskrypcję w `AccountModule.Use(...)` oraz dedykowany handler.
- Upewnij się, że testy integracyjne obejmują publikację i odbiór kluczowych zdarzeń (z użyciem `StubMessageBroker`).