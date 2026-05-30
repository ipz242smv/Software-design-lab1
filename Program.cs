using System.Collections;
using System.Text;

public static class StringExtensions
{
    public static string Reverse(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static int CountOccurrences(this string input, char character)
    {
        if (string.IsNullOrEmpty(input))
            return 0;

        return input.Count(c => c == character);
    }
}
public static class ArrayExtensions
{
    public static int CountValue<T>(this T[] array, T value) where T : IEquatable<T>
    {
        if (array == null || array.Length == 0)
            return 0;

        int count = 0;
        foreach (var item in array)
        {
            if (item.Equals(value))
                count++;
        }
        return count;
    }

    public static T[] GetUniqueElements<T>(this T[] array) where T : IEquatable<T>
    {
        if (array == null || array.Length == 0)
            return Array.Empty<T>();

        List<T> uniqueList = new List<T>();
        foreach (var item in array)
        {
            if (!uniqueList.Any(x => x.Equals(item)))
            {
                uniqueList.Add(item);
            }
        }
        return uniqueList.ToArray();
    }
}

public class ExtendedDictionaryElement<T, U, V>
{
    public T Key { get; private set; }
    public U Value1 { get; private set; }
    public V Value2 { get; private set; }

    public ExtendedDictionaryElement(T key, U value1, V value2)
    {
        Key = key;
        Value1 = value1;
        Value2 = value2;
    }

    public override string ToString()
    {
        return $"[Key: {Key}, Value1: {Value1}, Value2: {Value2}]";
    }
}

public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
{
    private List<ExtendedDictionaryElement<T, U, V>> elements = new List<ExtendedDictionaryElement<T, U, V>>();

    public void Add(T key, U value1, V value2)
    {
        if (ContainsKey(key))
        {
            throw new ArgumentException($"Елемент з ключем {key} вже існує");
        }

        elements.Add(new ExtendedDictionaryElement<T, U, V>(key, value1, value2));
    }

    public bool Remove(T key)
    {
        var element = elements.FirstOrDefault(e => e.Key.Equals(key));
        if (element != null)
        {
            return elements.Remove(element);
        }
        return false;
    }

    public bool ContainsKey(T key)
    {
        return elements.Any(e => e.Key.Equals(key));
    }

    public bool ContainsValue1(U value1)
    {
        return elements.Any(e => e.Value1.Equals(value1));
    }

    public bool ContainsValue2(V value2)
    {
        return elements.Any(e => e.Value2.Equals(value2));
    }

    public ExtendedDictionaryElement<T, U, V> this[T key]
    {
        get
        {
            var element = elements.FirstOrDefault(e => e.Key.Equals(key));
            if (element == null)
                throw new KeyNotFoundException($"Ключ {key} не знайдено");
            return element;
        }
    }

    public int Count => elements.Count;

    public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator()
    {
        return elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;
        Console.Write("Лабораторна робота №3.\nВиконав: Сорокін М.В., група ІПЗ-24-2\n");
        Console.WriteLine("1. Демонстрація методів розширення");
        Console.WriteLine();

        Console.WriteLine("Розширення для класу String:");
        string testString = "Hello, World!";

        Console.WriteLine($"Оригінальний рядок: {testString}");
        Console.WriteLine($"Інвертований рядок: {testString.Reverse()}");
        Console.WriteLine($"Кількість 'l' у рядку: {testString.CountOccurrences('l')}");
        Console.WriteLine($"Кількість 'o' у рядку: {testString.CountOccurrences('o')}");

        Console.WriteLine();

        Console.WriteLine("Розширення для масивів:");

        int[] intArray = { 1, 2, 3, 2, 4, 1, 5, 2 };
        Console.WriteLine($"Масив цілих чисел: [{string.Join(", ", intArray)}]");
        Console.WriteLine($"Кількість входжень числа 2: {intArray.CountValue(2)}");
        Console.WriteLine($"Кількість входжень числа 1: {intArray.CountValue(1)}");

        int[] uniqueIntArray = intArray.GetUniqueElements();
        Console.WriteLine($"Масив унікальних елементів: [{string.Join(", ", uniqueIntArray)}]");

        Console.WriteLine();

        string[] stringArray = { "apple", "banana", "apple", "orange", "banana" };
        Console.WriteLine($"Масив рядків: [{string.Join(", ", stringArray)}]");
        Console.WriteLine($"Кількість входжень 'apple': {stringArray.CountValue("apple")}");

        string[] uniqueStringArray = stringArray.GetUniqueElements();
        Console.WriteLine($"Масив унікальних елементів: [{string.Join(", ", uniqueStringArray)}]");

        Console.WriteLine();
        Console.WriteLine("2. Демонстрація узагальнених класів");
        Console.WriteLine();

        var extendedDict = new ExtendedDictionary<int, string, double>();

        Console.WriteLine("Додавання елементів до словника:");
        extendedDict.Add(1, "First", 100.5);
        extendedDict.Add(2, "Second", 200.3);
        extendedDict.Add(3, "Third", 300.7);
        extendedDict.Add(4, "First", 400.1);

        Console.WriteLine($"Загальна кількість елементів: {extendedDict.Count}");
        foreach (var element in extendedDict)
            Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine("Видалення елемента з ключем 2:");
        bool removed = extendedDict.Remove(2);
        Console.WriteLine($"Елемент видалено? {removed}");
        Console.WriteLine($"Загальна кількість елементів: {extendedDict.Count}");
        foreach (var element in extendedDict)
            Console.WriteLine(element);

        Console.WriteLine();

        Console.WriteLine("Перевірка наявності елементів:");
        Console.WriteLine($"Чи існує ключ 2? {extendedDict.ContainsKey(2)}");
        Console.WriteLine($"Чи існує ключ 4? {extendedDict.ContainsKey(4)}");
        Console.WriteLine($"Чи існує значення1 'First'? {extendedDict.ContainsValue1("First")}");
        Console.WriteLine($"Чи існує значення1 'Fifth'? {extendedDict.ContainsValue1("Fifth")}");
        Console.WriteLine($"Чи існує значення2 300.7? {extendedDict.ContainsValue2(300.7)}");
        Console.WriteLine($"Чи існує значення2 300? {extendedDict.ContainsValue2(300)}");

        Console.WriteLine();

        Console.WriteLine("Отримання елемента за ключем:");
        try
        {
            var element = extendedDict[3];
            Console.WriteLine($"Елемент з ключем 3: {element}");
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine();

        Console.WriteLine("Ітерація по словнику через foreach:");
        foreach (var elem in extendedDict)
        {
            Console.WriteLine($"Ключ: {elem.Key}, Value1: {elem.Value1}, Value2: {elem.Value2}");
        }

        Console.WriteLine();

        Console.WriteLine("ExtendedDictionary з іншими типами даних:");
        var stringDict = new ExtendedDictionary<string, int, DateTime>();

        stringDict.Add("user1", 25, new DateTime(1990, 1, 1));
        stringDict.Add("user2", 30, new DateTime(1985, 5, 15));

        foreach (var elem in stringDict)
        {
            Console.WriteLine($"Користувач: {elem.Key}, Вік: {elem.Value1}, Дата народження: {elem.Value2:yyyy-MM-dd}");
        }
    }
}