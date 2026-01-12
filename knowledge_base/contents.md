```
knowledge_base/
└─ docs/
   ├─ README.md                 # Ogólny opis aplikacji
   ├─ architecture.md            # Ogólny architektury aplikacji, wypisanie i krótki opis wszystkich ograniczonych kontekstów (bounded context), subcontextów, eventów, komend oraz zależności między modułami
   ├─ solution_structure.md  # Struktura solucji, folderów i projektów
   ├─ vision.md                      # Wizja stojąca za projektem, aktualny problem i sposób w jaki aplikacja ma go rozwiązać. Kto jest targetem aplikacji?
   ├─ tech_stack.md              # Wszystkie technologie użyte w aplikacji i cel ich użycia. Bazy danych, ORM, biblioteki, Języki programowania i frameworki
   ├─ bootstrapper/               # Projekt, który zawiera konfiguracje wszystkich usług aplikacji,  wstrzykiwanie zależności, punkt wejścia, middleware i rejestrację wszystkich modułów
   │  ├─ README.md
   │  ├─ architecture.md
   │  ├─ tech_stack.md
   ├─ api_gateway/               # Projekt, który jest publicznym API łączącym aplikację frontendową z naszym modularnym backendem
   │  ├─ README.md
   │  ├─ architecture.md
   │  └─ tech_stack.md
   └─ modules/
   │  │  ├─  account/
   │  │  │  ├─ README.md
   │  │  │  ├─ architecture.md
   │  │  │  ├─ events.md
   │  │  │  ├─ commands.md
   │  │  │  ├─ dependencies.md
   │  │  │  ├─ files-structure.md
   │  │  │  └─ tech_stack.md
   │  │  ├─  notifications/
   │  │  │  ├─ README.md
   │  │  │  ├─ architecture.md
   │  │  │  ├─ entities.md
   │  │  │  ├─ events.md
   │  │  │  ├─ commands.md
   │  │  │  ├─ dependencies.md
   │  │  │  ├─ files_structure.md
   │  │  │  └─ tech_stack.md
```