<conversation_summary>

<decisions>
1. Routing w MVP będzie uproszczony – bez pełnego silnika przesiadkowego między wieloma przewoźnikami. Trasy będą wyszukiwane w ramach jednej linii/jednego przewoźnika, a główną funkcją dla pasażera będzie szybkie sprawdzenie rozkładu i najbliższego odjazdu.
2. Dane (przystanki, linie, rozkłady) mogą być wprowadzane zarówno przez Admina, jak i Przewoźników, ale zakłada się, że Admin przygotuje dane startowe, a przewoźnicy później je utrzymują i aktualizują.
3. Rozkłady jazdy PDF pozostają w MVP jako funkcjonalność B2B dla Przewoźników (wydruk na słupki), z konfigurowalnym szablonem (logo przewoźnika, okres ważności, czytelny układ) oraz stopką „wygenerowano przez OwlMapper”.
4. Aplikacja będzie wspierała tryb offline dla pasażerów – cache statycznych rozkładów oraz wyraźne oznaczenie, czy użytkownik widzi dane rozkładowe (offline), czy dane „na żywo” (online z GPS).
5. Wyróżniono dwa typy powiadomień: (a) systemowe o opóźnieniach/awariach kursów wysyłane przez Przewoźnika, (b) przypomnienia o nadchodzących odjazdach realizowane lokalnie na urządzeniu („inteligentny budzik” dla tras cyklicznych).
6. Planowanie tras cyklicznych zostaje w MVP, w formie „inteligentnego budzika” – użytkownik definiuje trasę i dni tygodnia, a aplikacja sama przypomina przed odjazdem, nie obciążając nadmiernie backendu.
7. Zamiast pierwotnych prostych KPI (np. 100–300 wyszukiwań/miesiąc) przyjęto nowe wskaźniki sukcesu: m.in. 30% retencji tygodniowej, 1000 MAU w pilotażu oraz docelowe pokrycie kursów danymi GPS (min. 80% kursów z danymi na żywo).
8. Rozkłady mają być dynamiczne (real-time), realizowane przez: (a) aplikację / widok dla kierowcy wysyłający GPS; oraz (b) opcjonalną integrację z istniejącymi systemami przewoźnika poprzez prosty endpoint JSON (ID pojazdu, Lat, Lon, opóźnienie).
9. Do map i wyszukiwania tras wykorzystane zostaną gotowe rozwiązania oparte o OpenStreetMap oraz istniejące biblioteki/silniki, zamiast budowania własnego silnika mapowego.
10. Model biznesowy w MVP: aplikacja darmowa dla pasażerów i przewoźników; przychód wyłącznie z reklam (AdMob / AdSense + w przyszłości lokalni reklamodawcy).
11. Reklamy będą ograniczone do natywnych slotów (np. w listach wyników i w widoku szczegółów przystanku) oraz niewielkich banerów – bez pełnoekranowych interstitiali w krytycznych momentach (sprawdzanie odjazdu).
12. Dane przystanków będą utrzymywane osobno w środowiskach każdego przewoźnika (usunięcie/zmiana przystanku dotyczy tylko jego rozkładów), ale w aplikacji pasażera przystanki położone blisko siebie i o podobnej nazwie będą wizualnie agregowane do jednego punktu na mapie.
13. Architektura produktów: aplikacja mobilna (Android/iOS, technologia cross-platform) dla pasażera i kierowcy oraz panel webowy dla Admina i Przewoźników (zarządzanie rozkładami, przystankami, powiadomieniami i PDF).
14. Konta Przewoźników będą weryfikowane – rejestracja wymaga NIP/REGON, system wysyła link aktywacyjny na e-mail z CEIDG; kliknięcie linku aktywuje konto i informuje Admina. Konta pasażerskie mogą być zakładane automatycznie.
15. W MVP skupiamy się na ręcznym kreatorze rozkładów w panelu webowym (bez importu CSV/GTFS), priorytetem jest szybki, intuicyjny formularz do ręcznego wprowadzania danych.
16. Zakup biletów i bilety okresowe są poza zakresem MVP; struktura danych nie jest na razie projektowana pod e-commerce. Konto użytkownika jest potrzebne głównie do Ulubionych, miejsc typu Dom/Praca i tras cyklicznych.
17. Wybrano wstępnie pilotaż z jednym przewoźnikiem obsługującym cały powiat dzierżoniowski, zamiast jednego miasta, aby przetestować system w realnym, większym obszarze, ale z jednym partnerem.
18. Ustalono wstępne wymagania niefunkcjonalne: czas ładowania rozkładu poniżej 1,5 s, czas ładowania mapy poniżej 3 s oraz wysoka stabilność i brak krytycznych błędów przed wyjściem z pilotażu.
19. Zdefiniowano kryteria Go/No-Go po pilotażu: osiągnięcie KPI retencji, brak incydentów P1 w ostatnim miesiącu pilotażu oraz przychód reklamowy przynajmniej pokrywający koszty hostingu.
20. Kwestie prawne: dane rozkładów pozostają własnością przewoźników; OwlMapper ma niewyłączną licencję na ich przetwarzanie i prezentację w aplikacji i API.
21. Zakres funkcjonalności MVP ma zostać zamrożony; przesunięcia funkcji do V2 będą rozważane tylko w razie realnych blokad technicznych.
</decisions>

<matched_recommendations>
1. Ograniczenie złożoności silnika routingu w MVP do jednego przewoźnika / jednej linii oraz traktowanie aplikacji przede wszystkim jako szybkiej przeglądarki rozkładów – rekomendacja przyjęta.
2. Wstępne zasilenie systemu danymi przez Admina przy równoległej możliwości późniejszej edycji przez Przewoźników – rekomendacja częściowo przyjęta i rozszerzona.
3. Traktowanie generowania PDF jako kluczowej funkcji B2B dla przewoźników, z dopracowanym szablonem i brandingiem OwlMapper – rekomendacja przyjęta.
4. Wprowadzenie trybu offline z jasną prezentacją różnicy między danymi rozkładowymi a rzeczywistymi – rekomendacja przyjęta.
5. Rozdzielenie typów powiadomień (systemowe o opóźnieniach vs przypomnienia lokalne o odjazdach) – rekomendacja przyjęta.
6. Implementacja tras cyklicznych jako „inteligentnego budzika” zamiast rozbudowanego planera – rekomendacja przyjęta, mimo że funkcja nie została przesunięta do V2.
7. Przedefiniowanie KPI z prostych liczników wyszukiwań na wskaźniki retencji, adopcji i jakości GPS – rekomendacja przyjęta.
8. Budowa dynamicznych rozkładów w oparciu o prosty model danych + minimalne API JSON zamiast pełnego GTFS-RT – rekomendacja przyjęta.
9. Wykorzystanie gotowych narzędzi opartych na OpenStreetMap i standardach transportowych zamiast własnego silnika mapowego – rekomendacja przyjęta.
10. Wybór modelu reklamowego z integracją AdMob/AdSense i starannym umiejscowieniem reklam, bez agresywnych formatów – rekomendacja przyjęta.
11. Wizualne grupowanie przystanków różnych przewoźników na mapie przy jednoczesnym zachowaniu osobnych „prywatnych” przystanków w środowisku każdego przewoźnika – rekomendacja mapowa przyjęta, model danych świadomie zmodyfikowany.
12. Podział na aplikację mobilną (pasażer) i panel webowy (admin/przewoźnik) – rekomendacja przyjęta.
13. Weryfikacja przewoźnika przez dane z CEIDG i mail firmowy, z częściową automatyzacją procesu – rekomendacja przyjęta i doprecyzowana.
14. Skupienie się w MVP na ręcznym kreatorze rozkładów i odłożenie importu plików na później – rekomendacja przyjęta, z założeniem dobrze zaprojektowanego formularza wprowadzania danych.
15. Zdefiniowanie pilotażu jako kontrolowanego wdrożenia (1 przewoźnik, konkretny obszar) i użycie go do weryfikacji KPI – rekomendacja przyjęta, z korektą zakresu geograficznego.
16. Ujęcie wymagań niefunkcjonalnych (czasy odpowiedzi, stabilność) jako elementów krytycznych dla sukcesu produktu – rekomendacja przyjęta.
17. Jasne określenie własności danych rozkładów (przewoźnik) i licencji dla OwlMapper – rekomendacja przyjęta.
18. Zamrożenie zakresu MVP i koncentracja na jakości oraz UX/UI, zamiast dalszego dokładania funkcjonalności – rekomendacja przyjęta.
</matched_recommendations>

<prd_planning_summary>
**a. Główne wymagania funkcjonalne produktu (MVP)**

* **Role systemowe:**

  * Użytkownik (pasażer) – wyszukiwanie przystanków, podgląd rozkładów (statycznych i dynamicznych), dodawanie ulubionych przystanków i miejsc (Dom/Praca), definiowanie tras cyklicznych z powiadomieniami.
  * Przewoźnik – zarządzanie własnymi przystankami (w swoim „środowisku”), liniami, wariantami tras i rozkładami, generowanie PDF, wysyłka alertów o opóźnieniach/awariach, konfiguracja pojazdów i ewentualnych integracji GPS.
  * Admin – zarządzanie systemem, weryfikacja przewoźników, nadzór nad danymi i jakością, konfiguracja reklam.

* **Funkcje pasażera:**

  * Wyszukiwanie przystanków po nazwie i lokalizacji oraz prezentacja listy odjazdów (najbliższe kursy).
  * Widok mapy opartej na OpenStreetMap z agregowanymi punktami przystanków różnych przewoźników.
  * Prezentacja rozkładu w formie listy kursów (statycznie) z nakładką statusu „na żywo” (opóźnienia, pozycja pojazdu).
  * Dodawanie „Ulubionych” przystanków oraz własnych miejsc (Dom, Praca) i szybki dostęp do nich.
  * Definiowanie tras cyklicznych (np. Dom → Praca, określone dni i godziny) oraz otrzymywanie powiadomień przed odjazdem (lokalny „budzik”).
  * Obsługa trybu offline – dostęp do zapisanych rozkładów z jasnym oznaczeniem, że dane są statyczne (bez GPS).

* **Funkcje przewoźnika (panel webowy):**

  * Zarządzanie kontem przewoźnika po weryfikacji (NIP/REGON + mail CEIDG).
  * Dodawanie/edycja/usuwanie własnych przystanków, linii, wariantów tras i rozkładów jazdy poprzez prosty kreator.
  * Zarządzanie flotą pojazdów i przypisywanie ich do linii.
  * Konfiguracja źródeł danych GPS: poprzez integrację z zewnętrznym API (minimalny format JSON).
  * Generowanie rozkładów w PDF (z logo, okresem ważności, czytelnym układem i stopką OwlMapper).
  * Wysyłanie alertów/komunikatów dla pasażerów poprzez mechanizm „Szybkich Alertów” (predefiniowane szablony: opóźnienie, awaria, zmiana trasy).

* **Funkcje admina:**

  * Moderacja i weryfikacja przewoźników (proces CEIDG + aktywacja kont).
  * Nadzór nad ogólną jakością danych, monitoring błędów, KPI, konfiguracja reklam oraz ewentualna ręczna interwencja w dane (np. korekta widocznych błędów).
  * Konfiguracja slotów reklamowych i integracji z siecią reklamową.

* **Monetyzacja i reklamy:**

  * Integracja z AdMob/AdSense w aplikacji mobilnej.
  * Reklamy natywne i banery w niekrytycznych widokach (listy wyników, szczegóły przystanku).
  * Przygotowane miejsca na przyszłe kampanie lokalnych reklamodawców.

* **Niefunkcjonalne wymagania:**

  * Czas odpowiedzi: <1,5 s dla ładowania rozkładu, <3 s dla ładowania mapy.
  * Wysoka stabilność, szczególnie w zakresie GPS (brak częstych przerw w danych).
  * Skalowalność backendu pod rosnącą liczbę użytkowników i zapytań.

**b. Kluczowe historie użytkownika i ścieżki korzystania**

Przykładowe kluczowe ścieżki:

* **Pasażer: szybkie sprawdzenie odjazdu**

  * „Jako pasażer chcę szybko znaleźć najbliższy odjazd z mojego ulubionego przystanku, aby zdążyć na autobus.”
  * Flow: otwarcie aplikacji → ekran „Ulubione” / przystanek → rozkład z najbliższymi kursami (statyczne + status na żywo, jeśli dostępny).

* **Pasażer: nawigacja Dom–Praca**

  * „Jako pasażer chcę zapisać trasy Dom → Praca i dostawać przypomnienia przed odjazdem, aby nie spóźniać się do pracy.”
  * Flow: dodanie miejsca Dom/Praca → utworzenie trasy cyklicznej z dniami tygodnia i godziną → lokalne powiadomienia.

* **Pasażer: widok przystanku na mapie**

  * „Jako pasażer chcę zobaczyć przystanki w okolicy na mapie, aby wybrać ten, z którego odjeżdża mój autobus.”
  * Flow: widok mapy → klaster przystanków → rozwinięcie listy linii i kursów po kliknięciu.

* **Przewoźnik: zarządzanie rozkładem**

  * „Jako przewoźnik chcę łatwo dodać nową linię i rozkład, aby na bieżąco aktualizować informacje dla pasażerów.”
  * Flow: logowanie do panelu → kreator linii/tras → wprowadzanie godzin → zapis → automatyczna aktualizacja w aplikacji.

* **Przewoźnik: powiadomienie o opóźnieniu**

  * „Jako przewoźnik chcę szybko wysłać powiadomienie o opóźnieniu konkretnego kursu, aby pasażerowie byli poinformowani.”
  * Flow: wybór kursu w panelu → wybór szablonu alertu → wysyłka push do użytkowników korzystających z tej linii.

* **Admin: weryfikacja przewoźnika**

  * „Jako administrator chcę zweryfikować przewoźnika przez CEIDG, aby mieć pewność, że rozkłady są wiarygodne.”
  * Flow: nowa rejestracja przewoźnika → wysyłka linku na mail z CEIDG → kliknięcie → automatyczna aktywacja + powiadomienie admina.

**c. Kryteria sukcesu i sposoby ich mierzenia**

* **Retencja użytkowników:**

  * Cel: min. 30% użytkowników wraca do aplikacji w kolejnym tygodniu.
  * Pomiar: analityka aplikacji (np. cohorte tygodniowe, DAU/WAU).

* **Adopcja:**

  * Cel: 1000 aktywnych użytkowników miesięcznie (MAU) w powiecie dzierżoniowskim w ciągu 3 miesięcy pilotażu.
  * Pomiar: MAU, liczba unikalnych urządzeń/kont.

* **Jakość danych GPS:**

  * Cel: min. 80% kursów w systemie ma pokrycie danymi GPS w godzinach kursowania.
  * Pomiar: monitorowanie udziału kursów z aktywnym feedem GPS.

* **Stabilność i niezawodność:**

  * Cel: brak krytycznych incydentów P1 w ostatnim miesiącu pilotażu.
  * Pomiar: system zgłoszeń błędów, monitoring uptime, logi.

* **Monetyzacja:**

  * Cel: przychody z reklam pokrywają koszty hostingu (co najmniej break-even).
  * Pomiar: raporty AdMob/AdSense vs koszty infrastruktury.

* **Satysfakcja użytkowników (opcjonalnie, z pierwotnego opisu):**

  * Ankiety po pilotażu – odsetek użytkowników deklarujących, że aplikacja ułatwia dojazd i że chcą z niej korzystać na stałe.

**d. Kwestie wymagające dalszych doprecyzowań**

Zostaną rozwinięte w sekcji poniżej.

</prd_planning_summary>
</conversation_summary>
