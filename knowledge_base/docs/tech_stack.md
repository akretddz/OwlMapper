### Stos technologiczny backendu (OwlMapper)

| Technologia / komponent | Rola |
| --- | --- | --- |
| **C# 14 / .NET 10** | Język i runtime dla usług backendowych i narzędzi
| **ASP.NET Core Web API (modułowy monolit)** | Hostowanie API, moduły funkcjonalne |
| **PostgreSQL** | Główna baza danych relacyjna (schematy modułów) |
| **Entity Framework Core** | ORM, migracje, mapowanie encji, konteksty per moduł |
| **Migracje EF Core** | Zarządzanie schematem DB per moduł |
| **Unit of Work (własna implementacja)** | Spójne zapisy transakcyjne per moduł |
| **Outbox/Inbox (własna implementacja)** | Wzorzec niezawodnej wymiany komunikatów i idempotencji |
| **RabbitMQ** | Asynchroniczna komunikacja między modułami (publikacja/subskrypcja eventów) |
| **SignalR** | Komunikacja w czasie rzeczywistym (hubs) |
| **Background Services (Hosted Services)** | Zadania cykliczne/ciągłe (np. czyszczenie sag, generowanie szablonów powiadomień) |
| **Narzędzie Database Migrator** | Uruchamianie migracji w środowiskach |
| **PowerShell scripts** | CI/CD |
| **JWT + Refresh Tokens** | Uwierzytelnianie użytkowników, polityki haseł |