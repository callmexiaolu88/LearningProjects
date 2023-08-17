using System;
using Newtonsoft.Json;

namespace dynamicPropertyAccessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            TestClass instance = new TestClass();
            var property = "Name";
            var type = ModuleConfigHelper.GetItemType<TestClass>(property);
            Console.WriteLine($"{property}[{type.Name}]");
            var strValue = "{\"name\":123,\"id\":456}";
            var desvalue = JsonConvert.DeserializeObject(strValue, type);
            Console.WriteLine($"DeserializeObject: {desvalue}");
            var success = ModuleConfigHelper.TryGetItem(instance, property, out object value);
            Console.WriteLine($"Get: {property}: {value} [{success}]");
            success = ModuleConfigHelper.SetItem(instance, property, desvalue);
            Console.WriteLine($"Set: {property}->{desvalue} [{success}]");
            success = ModuleConfigHelper.TryGetItem(instance, property, out value);
            Console.WriteLine($"Get: {property}: {value} [{success}]");
        }
    }
}
