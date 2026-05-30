## 1. DRY (Don't Repeat Yourself)

**Опис:** Кожна частина знань повинна мати єдине, однозначне представлення в системі.

**Демонстрація:**

Метод [`CountValue<T>`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L26-L38) в [`ArrayExtensions`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L24-L38) є узагальненим, що дозволяє уникнути дублювання коду для різних типів:

```csharp
// ArrayExtensions.cs
public static int CountValue<T>(this T[] array, T value) where T : IEquatable<T>
```

Завдяки цьому той самий метод використовується як для `int[]`, так і для `string[]`:

```csharp
// Program.cs
Console.WriteLine($"Кількість входжень числа 2: {intArray.CountValue(2)}");
Console.WriteLine($"Кількість входжень 'apple': {stringArray.CountValue("apple")}");
```

Без узагальнень довелося б писати окремий метод для кожного типу.

---

## 2. KISS (Keep It Simple, Stupid)

**Опис:** Простота повинна бути ключовою метою в проєктуванні. Слід уникати зайвої складності.

**Демонстрація:**

Метод `Reverse` в `StringExtensions` реалізовано максимально просто — без зайвої логіки:

```csharp
// StringExtensions.cs
public static string Reverse(this string input)
{
    if (string.IsNullOrEmpty(input))
        return input;

    char[] charArray = input.ToCharArray();
    Array.Reverse(charArray);
    return new string(charArray);
}
```

Метод виконує рівно одну задачу і не містить зайвих абстракцій.

---

## 3. SRP — Single Responsibility Principle (принцип єдиної відповідальності)

**Опис:** Кожен клас або метод повинен мати лише одну відповідальність.

**Демонстрація:**

- `StringExtensions` — відповідає виключно за розширення рядків.
- `ArrayExtensions` — відповідає виключно за розширення масивів.
- `ExtendedDictionaryElement<T, U, V>` — відповідає за зберігання одного запису.
- `ExtendedDictionary<T, U, V>` — відповідає за управління колекцією.

```csharp
// StringExtensions.cs — лише операції над рядками
public static string Reverse(this string input) { ... }
public static int CountOccurrences(this string input, char character) { ... }

// ArrayExtensions.cs — лише операції над масивами
public static int CountValue<T>(this T[] array, T value) { ... }
public static T[] GetUniqueElements<T>(this T[] array) { ... }
```

---

## 4. Fail Fast

**Опис:** Помилки повинні виявлятися якомога раніше — на початку виконання методу, а не в середині.

**Демонстрація:**

Усі методи перевіряють вхідні дані на початку та негайно повертають результат або кидають виняток:

```csharp
// StringExtensions.cs
public static string Reverse(this string input)
{
    if (string.IsNullOrEmpty(input))
        return input; // ранній вихід
    ...
}

// ArrayExtensions.cs
public static int CountValue<T>(this T[] array, T value) where T : IEquatable<T>
{
    if (array == null || array.Length == 0)
        return 0; // ранній вихід
    ...
}

// ExtendedDictionary.cs
public void Add(T key, U value1, V value2)
{
    if (ContainsKey(key))
        throw new ArgumentException($"Елемент з ключем {key} вже існує"); // раннє виключення
    ...
}
```

---

## 5. ISP — Interface Segregation Principle (принцип розділення інтерфейсів)

**Опис:** Класи не повинні залежати від інтерфейсів, які вони не використовують.

**Демонстрація:**

`ExtendedDictionary<T, U, V>` реалізує тільки `IEnumerable<T>` — мінімальний інтерфейс, потрібний для `foreach`:

```csharp
// ExtendedDictionary.cs
public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
{
    public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator() { ... }
    IEnumerator IEnumerable.GetEnumerator() { ... }
}
```

Клас не реалізує `ICollection` або `IDictionary`, оскільки їхні методи не потрібні.

---

## 6. Meaningful Names (Значущі імена)

**Опис:** Імена змінних, методів і класів повинні чітко відображати їх призначення.

**Демонстрація:**

```csharp
// ArrayExtensions.cs
public static T[] GetUniqueElements<T>(...)  // чітко зрозуміло — повертає унікальні елементи

// ExtendedDictionaryElement.cs
public T Key { get; private set; }
public U Value1 { get; private set; }
public V Value2 { get; private set; }

// Program.cs
int[] uniqueIntArray = intArray.GetUniqueElements();
string[] uniqueStringArray = stringArray.GetUniqueElements();
```

Назви змінних та методів описують їх вміст без необхідності читати реалізацію.

---

## 7. Encapsulation (Інкапсуляція)

**Опис:** Внутрішній стан об'єкта повинен бути прихований і доступний лише через публічний інтерфейс.

**Демонстрація:**

У `ExtendedDictionaryElement` поля доступні лише для читання ззовні та встановлюються лише в конструкторі:

```csharp
// ExtendedDictionaryElement.cs
public T Key { get; private set; }
public U Value1 { get; private set; }
public V Value2 { get; private set; }

public ExtendedDictionaryElement(T key, U value1, V value2)
{
    Key = key;
    Value1 = value1;
    Value2 = value2;
}
```

У `ExtendedDictionary` внутрішній список `elements` є приватним:

```csharp
// ExtendedDictionary.cs
private List<ExtendedDictionaryElement<T, U, V>> elements = new List<...>();
```

---

## Issues (Запахи коду та порушення принципів)

### Issue #1 — Використання застарілого синтаксису повернення порожнього масиву

**Файл:** `ArrayExtensions.cs`

**Проблема:** Використання `new T[0]` замість сучаснішого `Array.Empty<T>()`.

```csharp
// До (запах коду):
return new T[0];

// Після (виправлення):
return Array.Empty<T>();
```

`Array.Empty<T>()` є рекомендованим підходом у сучасному C#, оскільки повертає кешований екземпляр і не виділяє зайву пам'ять.

---

### Issue #2 — Метод `DisplayAll` порушує SRP

**Файл:** `ExtendedDictionary.cs`

**Проблема:** Метод `DisplayAll` безпосередньо виводить дані в консоль, що порушує принцип єдиної відповідальності — клас колекції не повинен знати, як виводити себе на екран.

```csharp
// До (порушення SRP):
public void DisplayAll()
{
    Console.WriteLine($"Загальна кількість елементів: {Count}");
    foreach (var element in elements)
        Console.WriteLine(element);
}

// Після (виправлення — виводити через foreach у Program.cs):
Console.WriteLine($"Загальна кількість елементів: {extendedDict.Count}");
foreach (var element in extendedDict)
    Console.WriteLine(element);
```

---

### Issue #3 — Неефективний пошук унікальних елементів (O(n²))

**Файл:** `ArrayExtensions.cs`

**Проблема:** Метод `GetUniqueElements` використовує `List<T>` з лінійним пошуком через `.Any(...)`, що дає складність O(n²).

```csharp
// До (неефективно, O(n²)):
List<T> uniqueList = new List<T>();
foreach (var item in array)
{
    if (!uniqueList.Any(x => x.Equals(item)))
        uniqueList.Add(item);
}

// Після (виправлення — HashSet для O(n)):
public static T[] GetUniqueElements<T>(this T[] array) where T : IEquatable<T>
{
    if (array == null || array.Length == 0)
        return Array.Empty<T>();

    var seen = new HashSet<T>();
    var uniqueList = new List<T>();
    foreach (var item in array)
    {
        if (seen.Add(item))
            uniqueList.Add(item);
    }
    return uniqueList.ToArray();
}
```
