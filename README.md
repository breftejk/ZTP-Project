# Opis implementacji wzorców projektowych

## Wzorzec Fabryka (Factory)
### Funkcjonalność:
Fabryka w tym rozwiązaniu jest zaimplementowana jako klasa `WordServiceFactory`. Jej zadaniem jest dynamiczne tworzenie instancji serwisów `WordService` dla różnych języków. W przypadku, gdy serwis dla danego języka już istnieje, fabryka zwraca jego wcześniejszą instancję, zamiast tworzyć nowy obiekt.

### Założenia wzorca:
1. **Dynamiczne tworzenie obiektów:**
   Fabryka umożliwia dynamiczne tworzenie serwisów na podstawie argumentu (`language`). Dzięki temu kod jest elastyczny i może obsługiwać wiele języków bez zmiany logiki kontrolera.

2. **Oddzielenie logiki tworzenia:**
   Klient (kontroler) nie musi znać szczegółów, jak i gdzie dane są przechowywane ani jak serwis jest tworzony. Fabryka izoluje tę logikę.

3. **Cache obiektów:**
   Fabryka przechowuje już utworzone obiekty w słowniku (`Dictionary<string, IWordService>`), co redukuje koszt ponownego wczytywania danych z plików.

---

## Wzorzec Fasada (Facade)
### Funkcjonalność:
Fasada jest zaimplementowana w klasie `WordFacade`. Zapewnia uproszczony interfejs do zarządzania danymi w różnych językach. Używa fabryki do tworzenia odpowiednich serwisów i deleguje do nich wywołania metod takich jak `GetAllWords` czy `GetTranslations`.

### Założenia wzorca:
1. **Ukrycie złożoności:**
   Fasada ukrywa szczegóły implementacji fabryki i serwisów. Klient (kontroler) komunikuje się tylko z fasadą i nie musi znać szczegółów dotyczących plików JSON czy procesu ich ładowania.

2. **Jednopunktowy dostęp:**
   `WordFacade` zapewnia jeden spójny interfejs do wykonywania operacji, takich jak pobieranie wszystkich słów czy tłumaczeń dla danego języka.

3. **Modularność i skalowalność:**
   Dodanie nowego języka lub zmiana logiki serwisu wymaga modyfikacji jedynie w fabryce lub serwisie. Interfejs fasady pozostaje niezmieniony.

---

## Wzorzec Dekorator (Decorator)
### Funkcjonalność:
Dekorator jest zaimplementowany w klasie `AuthorizationServiceDecorator`. Rozszerza funkcjonalność podstawowej usługi `IAuthorizationService`, dodając logowanie do procesu autoryzacji.

### Założenia wzorca:
1. **Rozszerzalność:**
   Dekorator umożliwia dodanie nowych funkcji (np. logowanie procesu autoryzacji) bez modyfikowania podstawowej implementacji `IAuthorizationService`.

2. **Transparentność:**
   Klient używający `AuthorizationServiceDecorator` nie musi znać szczegółów implementacji logowania – może traktować dekoratora jak zwykłą implementację `IAuthorizationService`.

3. **Zasada otwarte-zamknięte (Open/Closed Principle):**
   Dekorator pozwala na rozszerzanie funkcjonalności bez konieczności zmieniania istniejącej klasy.

### Przykład w implementacji:
1. `AuthorizationServiceDecorator` przyjmuje w konstruktorze instancję `IAuthorizationService` (kompozycja) oraz logger.
2. Metoda `AuthorizeAsync` loguje rozpoczęcie procesu autoryzacji, deleguje rzeczywisty proces autoryzacji do `_inner.AuthorizeAsync`, a następnie loguje jego zakończenie.
3. Klasa `AuthorizationFilter` korzysta z `IAuthorizationService`, nie wiedząc, czy używa dekoratora, czy podstawowej implementacji.

---

## Korzyści z połączenia Fabryki, Fasady i Dekoratora

### Elastyczność:
- **Fabryka:** Dynamicznie tworzy instancje serwisów na podstawie języka.
- **Fasada:** Ujednolica interfejs dostępu do danych, niezależnie od szczegółów implementacji.
- **Dekorator:** Umożliwia łatwe rozszerzenie funkcjonalności autoryzacji (np. logowanie) bez zmiany istniejących klas.

### Oddzielenie odpowiedzialności:
- Fabryka zajmuje się zarządzaniem instancjami serwisów.
- Fasada upraszcza dostęp do funkcji aplikacji.
- Dekorator dodaje logikę dodatkową (logowanie) w izolowany sposób.

### Modułowość:
Każdy wzorzec spełnia odrębną rolę, co sprawia, że system jest łatwy do rozbudowy, testowania i utrzymania.