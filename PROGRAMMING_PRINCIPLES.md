## 1. DRY (Don't Repeat Yourself)

**Опис:** Кожна частина знань повинна мати єдине, однозначне представлення в системі.

**Демонстрація:**

Метод [`CountValue<T>`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L26-L38) в [`ArrayExtensions`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L24-L38) є узагальненим, що дозволяє уникнути дублювання коду для різних типів:

```csharp
public static int CountValue<T>(this T[] array, T value) where T : IEquatable<T>
```

Завдяки цьому той самий метод використовується як для [`int[]`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L172), так і для [`string[]`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L182).

Без узагальнень довелося б писати окремий метод для кожного типу.

---

## 2. KISS (Keep It Simple, Stupid)

**Опис:** Простота повинна бути ключовою метою в проєктуванні. Слід уникати зайвої складності.

**Демонстрація:**

Метод [`Reverse`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L6-L14) в [`StringExtensions`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L4-L23) реалізовано максимально просто — без зайвої логіки.

Метод виконує рівно одну задачу і не містить зайвих абстракцій.

---

## 3. SRP — Single Responsibility Principle (принцип єдиної відповідальності)

**Опис:** Кожен клас або метод повинен мати лише одну відповідальність.

**Демонстрація:**

- [`StringExtensions`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L4-L23) — відповідає виключно за розширення рядків.
- [`ArrayExtensions`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L24-L55) — відповідає виключно за розширення масивів.
- [`ExtendedDictionaryElement<T, U, V>`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L57-L74) — відповідає за зберігання одного запису.
- [`ExtendedDictionary<T, U, V>`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L76-L146) — відповідає за управління колекцією.

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

[`ExtendedDictionary<T, U, V>`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L76-L146) реалізує тільки `IEnumerable<T>` — мінімальний інтерфейс, потрібний для `foreach`:

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

У [`ExtendedDictionaryElement`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L57-L74) поля доступні лише для читання ззовні та встановлюються лише в конструкторі:

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

У [`ExtendedDictionary`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L76-L147) внутрішній список [`elements`](https://github.com/ipz242smv/Software-design-lab1/blob/37a2ef857ad036026fe86eb1c02acc42b6d2f2a4/Program.cs#L78) є приватним.
