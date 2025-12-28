## Konwencje Nazewnictwa

- Projekty - PascalCase
- Klasy - PascalCase, nazwy powinny być rzeczownikami
- Interfejsy - PascalCase z prefiksem `I`
- Metody - PascalCase, nazwy powinny być czasownikami
- Publiczne pola i zmienne - PascalCase
- Internal pola i zmienne - PascalCase
- Prywatne pola i zmienne - prefiks '_' i camelCase
- Metody Testowe: Opisowe nazwy oddzielone '_'.

### Formatowanie Instrukcji Warunkowych
Instrukcje warunkowe (`if`/`else`) zawsze otaczamy klamrami `{}` i oddzielamy spacjami od reszty kodu dla większej przejrzystości.

**DOBRZE:**
```csharp
private static int GetNumber()
{
    var maxNumber = GetMaxNumber();

    if (maxNumber > 1)
    {
        maxNumber++;
    }
    else
    {
        maxNumber--;
    }
    
    return maxNumber;
}
```

**ŹLE:**
```csharp
private static int GetNumber()
{
var maxNumber = GetMaxNumber();
if (maxNumber > 1)
maxNumber++;
else
maxNumber--;
return maxNumber;
}
```

### Wyrównywanie zmiennych
Deklaracje zmiennych i stałych wyrównuj względem operatorów `=`, `_`, `:`, `{` (tylko w {get; set;})

**Przykłady**
```csharp
var firstNumber  = 1;
var secondNumber = 2;
var tenthNumber  = 10;

const string AppName      = "SampleApp";
const string DefaultUser  = "admin";
const int    DefaultLimit = 100;

var wrongDeclaration = new DeclarationModelPublic(
                otherContactPhone: null,
                           declarationId: Guid.NewGuid(),
    otherContactAddressStreet: null,
               declarationStartDate: _driverRecordTwo.DeclarationDateTime.AddDays(1).ToString("yyyy-MM-dd"), 
        otherContactAddressCity: null);

public sealed class DeclarationAndDriverData
{
    public string   FirstName                     { get; set; }
    public string   LastName                      { get; set; }
    public DateTime BirthDateTime                 { get; set; }
    public DateTime DeclarationDateTime           { get; set; }
    public DateTime DeclarationExpirationDateTime { get; set; }
    public List     Countries                     { get; set; }
    public List     RegisterNumbers               { get; set; }
    public DeclarationStatusEnum Status           { get; set; }
}
```

---

### Skracanie metod
Jeśli metoda jedynie zwraca obiekty to użyj skrótu:

**Metoda**
```csharp
public int Double(int a)
{
  var b = a * 2;

  return b
}
```

**Skrót**
```csharp
public int Double(int a)
  => a * 2;
```

## Błędy, wyjątki i przypadki brzegowe

### Obsługa wyjątków
Używaj try/catch jedynie na najwyższym poziomie wywołania do przechwytywania jedynie niespodziewanych wyjątków. Nie wyrzucamy go tylko logujemy błąd z pomocą `Seriloga`.

Nie używaj wyjątków do kontrolowania przepływu programu. 

### Wydajność
W przypadku wielokrotnego łączenia ciągów znaków należy używać StringBuilder zamiast operatora +.
Unikaj zbędnego przydzielania obiektów, szczególnie w pętlach.


## Inne Instrukcje

- Nazywaj zmienne w zrozumiały sposób, tak żeby było wiadomo czym ta zmienna jest
- Przy pisaniu metody, pamiętaj o walidacji danych wejściowych (chyba, że to nie jest konieczne)
- Wszystkie publiczne niestatyczne metody oznaczaj virtual.
- Stałych wartości typów prymitywnych nigdy nie hardkoduj w kodzie, tylko umieść w pasującej klasie statycznej w folderze Consts i do nich się odnoś. Nigdy nie umieszczaj w nich metod, tylko stałe wartości!
- Dokumentację piszemy wyłącznie w plikach Markdown, w języku polskim.
- Rozmawiaj ze mną tylko po polsku.
- Przed instrukcją ze słowem kluczowym `return` jedna oddzielająca linia odstępu
- Usuwaj nieużywane usingi, klasy, zmienne.
- Powtarzający się kod -> do osobnych metod/klas.
- Nazwy list -> końcówka, np. `DriversList`.
- Nazwy słowników -> końcówka Dictionary, np. `StatusesDictionary`.
- Do stringów używaj string.IsNullOrWhiteSpace(word).
- Stosuj SOLID z naciskiem na testowalność.
- Wyrównuj wcięcia.
- Używaj nameof zamiast literałów łańcuchowych przy odwoływaniu się do nazw członków.
