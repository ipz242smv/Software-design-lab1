# PROGRAMMING_PRINCIPLES.md

## Принципи програмування в проєкті

### 1. DRY (Don't Repeat Yourself)

**Опис принципу:** Код не повинен містити повторюваної логіки. Кожна частина знання має бути представлена в єдиному місці.

**Дотримання в проєкті:**

- **Єдина логіка створення елементів словника:** Клас `ExtendedDictionary` використовує конструктор `ExtendedDictionaryElement<T, U, V>` в єдиному місці - методі `Add`. Це запобігає дублюванню коду створення об'єктів.  
  *Рядки: 70-76*

- **Перевикористання методу `ContainsKey`:** Метод `Remove` використовує вже існуючий метод `ContainsKey` для перевірки, а не дублює логіку пошуку.  
  *Рядки: 78-84*

- **Уніфікована логіка порівняння:** Всі методи пошуку (`ContainsKey`, `ContainsValue1`, `ContainsValue2`) використовують однаковий підхід через LINQ `Any()`, не дублюючи код перевірки.  
  *Рядки: 86-96*

```csharp
// Приклад з коду: єдина логіка створення елемента
public void Add(T key, U value1, V value2)
{
    if (ContainsKey(key))
    {
        throw new ArgumentException($"Елемент з ключем {key} вже існує");
    }
    elements.Add(new ExtendedDictionaryElement<T, U, V>(key, value1, value2));
}
