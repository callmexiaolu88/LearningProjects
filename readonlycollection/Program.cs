using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var list1 = GetEnumerableStrings();
        print(list1);
        var typeName = typeof(Dictionary<string, int>).Name;
        System.Console.WriteLine(typeName[..typeName.IndexOf("`")]);
        if (list1 is ICollection<string> collection)
        {
           var types= typeof(Program).Assembly.GetTypes();
            collection.Add("IEnumerable");
            print(list1);
        }

        var list2 = GetReadOnlyStrings();
        print(list2);
        if (list2 is ICollection<string> collection2)
        {
            collection2.Add("IReadOnlyCollection");
            print(list2);
        }
    }

    public static IEnumerable<string> GetEnumerableStrings()
    {
        List<string> list = new List<string>() { "a", "b", "c", "d", "e", "f" };
        return new ReadOnlyCollection<string>(list);;
    }

    public static IReadOnlyCollection<string> GetReadOnlyStrings()
    {
        List<string> list = new List<string>() { "a", "b", "c", "d", "e", "f" };
        
        return new ReadOnlyCollection<string>(list);
    }

    private static void print(IEnumerable<string> source)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in source)
        {
            sb.Append(item + ",");
        }
        System.Console.WriteLine(sb.ToString());
    }

}